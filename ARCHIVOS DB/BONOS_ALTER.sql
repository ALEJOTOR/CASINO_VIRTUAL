-- ================================================================
-- MÓDULO 4: CONSTRAINTS E ÍNDICE PARA LOG_EVENTOS
-- ================================================================

-- Índice compuesto para búsquedas frecuentes por fecha + nivel
CREATE INDEX IDX_LOG_FECHA_NIVEL
    ON SEBAS_CASINO.LOG_EVENTOS (FECHA DESC, NIVEL, TIPO_EVENTO)
    TABLESPACE CASINO_VIRTUAL;

-- Normalizar niveles existentes antes de agregar el CHECK
UPDATE SEBAS_CASINO.LOG_EVENTOS SET NIVEL = 'INFO' WHERE NIVEL IS NULL OR NIVEL NOT IN ('INFO','WARN','ERROR','CRITICAL');
UPDATE SEBAS_CASINO.LOG_EVENTOS SET TIPO_EVENTO = 'error_sistema' WHERE TIPO_EVENTO IS NULL;
COMMIT;

-- CHECK constraint para nivel
ALTER TABLE SEBAS_CASINO.LOG_EVENTOS
    ADD CONSTRAINT CHK_LOG_NIVEL
    CHECK (NIVEL IN ('INFO', 'WARN', 'ERROR', 'CRITICAL'));

-- CHECK constraint para tipos de evento estándar
ALTER TABLE SEBAS_CASINO.LOG_EVENTOS
    ADD CONSTRAINT CHK_LOG_TIPO_EVENTO
    CHECK (TIPO_EVENTO IN (
        'login', 'logout', 'login_fallido',
        'registro_usuario', 'bloqueo_usuario', 'eliminacion_usuario',
        'deposito', 'retiro', 'apuesta_alta',
        'bono_aplicado', 'bono_removido',
        'error_sistema', 'acceso_admin',
        'cambio_contrasena', 'cambio_estado'
    ));

-- ================================================================
-- MÓDULO 5: MODIFICACIONES A TABLA BONOS
-- ================================================================

-- Agregar fecha de expiración al catálogo de bonos
ALTER TABLE SEBAS_CASINO.BONOS ADD (
    FECHA_INICIO    DATE DEFAULT SYSDATE,
    FECHA_FIN       DATE,
    USOS_MAXIMOS    NUMBER(5) DEFAULT NULL,
    USOS_ACTUALES   NUMBER(5) DEFAULT 0
);

-- Actualizar datos existentes (compatibilidad)
UPDATE SEBAS_CASINO.BONOS SET FECHA_INICIO = SYSDATE WHERE FECHA_INICIO IS NULL;
COMMIT;

-- Agregar fecha de expiración a la asignación usuario-bono
ALTER TABLE SEBAS_CASINO.USUARIO_BONOS ADD (
    FECHA_EXPIRACION    DATE,
    BONO_CONSUMIDO      NUMBER(1) DEFAULT 0
);

-- ================================================================
-- MÓDULO 5: PROCEDIMIENTO PR_REVOCAR_BONO
-- ================================================================

CREATE OR REPLACE PROCEDURE SEBAS_CASINO.PR_REVOCAR_BONO (
    p_id_usuario_bono   IN  NUMBER,
    p_motivo            IN  VARCHAR2 DEFAULT NULL,
    p_resultado         OUT VARCHAR2
) AS
    v_id_usuario    NUMBER;
    v_monto         NUMBER(12,2);
    v_estado        VARCHAR2(20);
BEGIN
    SELECT id_usuario, monto_aplicado, estado
      INTO v_id_usuario, v_monto, v_estado
      FROM usuario_bonos
     WHERE id_usuario_bono = p_id_usuario_bono;

    IF v_estado != 'aplicado' THEN
        p_resultado := 'El bono no está en estado aplicado.';
        RETURN;
    END IF;

    -- Revertir el saldo: insertar transacción negativa
    INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
    VALUES (seq_transacciones.NEXTVAL, v_id_usuario, 'retiro', v_monto,
            SYSTIMESTAMP, NVL(p_motivo, 'Revocación de bono por administrador'));

    -- Marcar como revocado
    UPDATE usuario_bonos SET estado = 'revocado' WHERE id_usuario_bono = p_id_usuario_bono;

    pr_registrar_log('bono_removido', 'WARN', v_id_usuario, NULL, 'BONOS',
        'Bono revocado. ID: ' || p_id_usuario_bono || '. Motivo: ' || NVL(p_motivo, 'Sin motivo'));
    COMMIT;
    p_resultado := 'Guardado correctamente.';
EXCEPTION
    WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error: ' || SQLERRM;
END PR_REVOCAR_BONO;
/

-- ================================================================
-- MÓDULO 5: ACTUALIZAR VISTA VW_BONOS_USUARIO
-- ================================================================

CREATE OR REPLACE VIEW SEBAS_CASINO.VW_BONOS_USUARIO AS
SELECT
    ub.id_usuario_bono,
    ub.id_usuario,
    u.username,
    u.nombre_1 || ' ' || u.apellido_1 AS nombre_usuario,
    b.nombre                           AS nombre_bono,
    b.tipo                             AS tipo_bono,
    b.valor                            AS valor_bono,
    ub.monto_aplicado,
    ub.fecha_aplicado,
    ub.fecha_expiracion,
    ub.estado,
    ub.descripcion,
    ub.bono_consumido,
    CASE WHEN ub.fecha_expiracion IS NULL THEN 'Permanente'
         WHEN ub.fecha_expiracion < SYSDATE THEN 'Expirado'
         ELSE 'Vigente'
    END AS estado_vigencia
FROM usuario_bonos ub
JOIN usuarios u ON ub.id_usuario = u.id_usuario
JOIN bonos    b ON ub.id_bono    = b.id_bono
ORDER BY ub.fecha_aplicado DESC;

-- ================================================================
-- MÓDULO 6: FUNCIÓN FN_CALCULAR_GANANCIA_CON_BONO
-- ================================================================

CREATE OR REPLACE FUNCTION SEBAS_CASINO.FN_CALCULAR_GANANCIA_CON_BONO(
    p_id_usuario    IN NUMBER,
    p_ganancia_base IN NUMBER
) RETURN NUMBER AS
    v_multiplicador  NUMBER(12,4) := 1;
    v_bono_tipo      VARCHAR2(50);
    v_bono_valor     NUMBER(12,2);
BEGIN
    -- Buscar si tiene un bono de tipo 'multiplicador' o 'porcentaje' activo y vigente
    SELECT b.tipo, b.valor INTO v_bono_tipo, v_bono_valor
      FROM usuario_bonos ub
      JOIN bonos b ON ub.id_bono = b.id_bono
     WHERE ub.id_usuario = p_id_usuario
       AND ub.estado = 'aplicado'
       AND b.tipo IN ('multiplicador', 'porcentaje')
       AND (ub.fecha_expiracion IS NULL OR ub.fecha_expiracion >= SYSDATE)
       AND ROWNUM = 1
     ORDER BY ub.fecha_aplicado DESC;

    IF v_bono_tipo = 'multiplicador' THEN
        RETURN p_ganancia_base * v_bono_valor;
    ELSIF v_bono_tipo = 'porcentaje' THEN
        RETURN p_ganancia_base + (p_ganancia_base * v_bono_valor / 100);
    END IF;
    RETURN p_ganancia_base;
EXCEPTION
    WHEN NO_DATA_FOUND THEN RETURN p_ganancia_base;
END FN_CALCULAR_GANANCIA_CON_BONO;
/
