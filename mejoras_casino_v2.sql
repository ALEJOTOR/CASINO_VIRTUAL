-- ============================================================
-- mejoras_casino_v2.sql
-- Descripcion: Nuevos objetos + modificaciones para CASINO VIRTUAL
--   - Tablas: bonos, usuario_bonos, metodos_pago, depositos,
--             retiros, apuestas, log_eventos
--   - Secuencias, funciones, vistas, packages
-- Esquema: SEBAS_CASINO, Tablespace: CASINO_VIRTUAL
-- ============================================================

-- ============================================================
-- 0. CREACION DE SECUENCIAS (si no existen)
-- ============================================================
DECLARE
    v_count NUMBER;
BEGIN
    -- seq_bonos
    SELECT COUNT(*) INTO v_count FROM user_sequences WHERE sequence_name = 'SEQ_BONOS';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE SEQUENCE seq_bonos MINVALUE 1 START WITH 1 INCREMENT BY 1 CACHE 20';
    END IF;

    -- seq_usuario_bonos
    SELECT COUNT(*) INTO v_count FROM user_sequences WHERE sequence_name = 'SEQ_USUARIO_BONOS';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE SEQUENCE seq_usuario_bonos MINVALUE 1 START WITH 1 INCREMENT BY 1 CACHE 20';
    END IF;

    -- seq_metodos_pago
    SELECT COUNT(*) INTO v_count FROM user_sequences WHERE sequence_name = 'SEQ_METODOS_PAGO';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE SEQUENCE seq_metodos_pago MINVALUE 1 START WITH 1 INCREMENT BY 1 CACHE 20';
    END IF;

    -- seq_depositos
    SELECT COUNT(*) INTO v_count FROM user_sequences WHERE sequence_name = 'SEQ_DEPOSITOS';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE SEQUENCE seq_depositos MINVALUE 1 START WITH 1 INCREMENT BY 1 CACHE 20';
    END IF;

    -- seq_retiros
    SELECT COUNT(*) INTO v_count FROM user_sequences WHERE sequence_name = 'SEQ_RETIROS';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE SEQUENCE seq_retiros MINVALUE 1 START WITH 1 INCREMENT BY 1 CACHE 20';
    END IF;

    -- seq_apuestas
    SELECT COUNT(*) INTO v_count FROM user_sequences WHERE sequence_name = 'SEQ_APUESTAS';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE SEQUENCE seq_apuestas MINVALUE 1 START WITH 1 INCREMENT BY 1 CACHE 20';
    END IF;

    -- seq_log_eventos
    SELECT COUNT(*) INTO v_count FROM user_sequences WHERE sequence_name = 'SEQ_LOG_EVENTOS';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE SEQUENCE seq_log_eventos MINVALUE 1 START WITH 1 INCREMENT BY 1 CACHE 20';
    END IF;
END;
/

-- ============================================================
-- 1. CREACION DE TABLAS (si no existen)
-- ============================================================

-- 1a. bonos
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM user_tables WHERE table_name = 'BONOS';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE TABLE bonos (
            id_bono     NUMBER        PRIMARY KEY,
            nombre      VARCHAR2(100) NOT NULL,
            tipo        VARCHAR2(50)  NOT NULL,
            valor       NUMBER(12,2)  NOT NULL,
            descripcion VARCHAR2(500),
            activo      NUMBER(1)     DEFAULT 1 NOT NULL
        ) TABLESPACE CASINO_VIRTUAL';
    END IF;
END;
/

-- 1b. metodos_pago
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM user_tables WHERE table_name = 'METODOS_PAGO';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE TABLE metodos_pago (
            id_metodo   NUMBER        PRIMARY KEY,
            tipo        VARCHAR2(50)  NOT NULL,
            descripcion VARCHAR2(200),
            activo      NUMBER(1)     DEFAULT 1 NOT NULL
        ) TABLESPACE CASINO_VIRTUAL';
    END IF;
END;
/

-- 1c. depositos
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM user_tables WHERE table_name = 'DEPOSITOS';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE TABLE depositos (
            id_deposito         NUMBER        PRIMARY KEY,
            id_usuario          NUMBER        NOT NULL,
            id_metodo           NUMBER,
            monto               NUMBER(12,2)  NOT NULL,
            estado              VARCHAR2(20)  DEFAULT ''pendiente'' NOT NULL,
            fecha_solicitud     TIMESTAMP     DEFAULT SYSTIMESTAMP,
            fecha_procesamiento TIMESTAMP,
            referencia_wompi    VARCHAR2(100),
            error_detalle       VARCHAR2(500),
            descripcion         VARCHAR2(500),
            CONSTRAINT fk_dep_usr FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
            CONSTRAINT fk_dep_met FOREIGN KEY (id_metodo)  REFERENCES metodos_pago(id_metodo)
        ) TABLESPACE CASINO_VIRTUAL';
    END IF;
END;
/

-- 1d. retiros
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM user_tables WHERE table_name = 'RETIROS';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE TABLE retiros (
            id_retiro           NUMBER        PRIMARY KEY,
            id_usuario          NUMBER        NOT NULL,
            id_metodo           NUMBER,
            monto               NUMBER(12,2)  NOT NULL,
            estado              VARCHAR2(20)  DEFAULT ''pendiente'' NOT NULL,
            fecha_solicitud     TIMESTAMP     DEFAULT SYSTIMESTAMP,
            fecha_procesamiento TIMESTAMP,
            id_admin_revisor    NUMBER,
            referencia_wompi    VARCHAR2(100),
            motivo_rechazo      VARCHAR2(500),
            CONSTRAINT fk_ret_usr FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
            CONSTRAINT fk_ret_met FOREIGN KEY (id_metodo)  REFERENCES metodos_pago(id_metodo)
        ) TABLESPACE CASINO_VIRTUAL';
    END IF;
END;
/

-- 1e. usuario_bonos
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM user_tables WHERE table_name = 'USUARIO_BONOS';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE TABLE usuario_bonos (
            id_usuario_bono NUMBER        PRIMARY KEY,
            id_usuario      NUMBER        NOT NULL,
            id_bono         NUMBER        NOT NULL,
            monto_aplicado  NUMBER(12,2)  NOT NULL,
            fecha_aplicado  TIMESTAMP     DEFAULT SYSTIMESTAMP,
            estado          VARCHAR2(20)  DEFAULT ''aplicado'',
            descripcion     VARCHAR2(500),
            CONSTRAINT fk_ub_usr FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
            CONSTRAINT fk_ub_bon FOREIGN KEY (id_bono)    REFERENCES bonos(id_bono)
        ) TABLESPACE CASINO_VIRTUAL';
    END IF;
END;
/

-- 1f. apuestas
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM user_tables WHERE table_name = 'APUESTAS';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE TABLE apuestas (
            id_apuesta     NUMBER        PRIMARY KEY,
            id_partida     NUMBER        NOT NULL,
            tipo_apuesta   VARCHAR2(50)  NOT NULL,
            numero_apuesta NUMBER,
            monto          NUMBER(12,2)  NOT NULL,
            multiplicador  NUMBER(12,2)  DEFAULT 1,
            ganancia       NUMBER(12,2)  DEFAULT 0,
            resultado      VARCHAR2(20)  DEFAULT ''perdida'',
            CONSTRAINT fk_apu_par FOREIGN KEY (id_partida) REFERENCES partidas(id_partida)
        ) TABLESPACE CASINO_VIRTUAL';
    END IF;
END;
/

-- 1g. log_eventos
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM user_tables WHERE table_name = 'LOG_EVENTOS';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE TABLE log_eventos (
            id_log       NUMBER        PRIMARY KEY,
            tipo_evento  VARCHAR2(50)  NOT NULL,
            nivel        VARCHAR2(10)  NOT NULL,
            id_usuario   NUMBER,
            ip_origen    VARCHAR2(50),
            modulo       VARCHAR2(100) NOT NULL,
            descripcion  VARCHAR2(2000),
            fecha        TIMESTAMP     DEFAULT SYSTIMESTAMP
        ) TABLESPACE CASINO_VIRTUAL';
    END IF;
END;
/

-- ============================================================
-- 2. FUNCION: FN_CALCULAR_CASHBACK
-- ============================================================

CREATE OR REPLACE EDITIONABLE FUNCTION "SEBAS_CASINO"."FN_CALCULAR_CASHBACK" (
    p_id_usuario IN NUMBER
) RETURN NUMBER AS
    v_perdidas    NUMBER := 0;
    v_ganancias   NUMBER := 0;
    v_porcentaje  NUMBER := 0;
BEGIN
    SELECT NVL(SUM(apuesta), 0), NVL(SUM(ganancia), 0)
    INTO v_perdidas, v_ganancias
    FROM partidas
    WHERE id_usuario = p_id_usuario
      AND fecha >= TRUNC(SYSDATE) - 7
      AND id_estado = 3;

    SELECT NVL(valor, 0) INTO v_porcentaje
    FROM bonos WHERE tipo = 'cashback' AND activo = 1 AND ROWNUM = 1;

    IF v_perdidas > v_ganancias THEN
        RETURN ROUND((v_perdidas - v_ganancias) * (v_porcentaje / 100), 2);
    END IF;
    RETURN 0;
END fn_calcular_cashback;
/

-- ============================================================
-- 3. PROCEDIMIENTOS EXISTENTES (recrear si faltan)
-- ============================================================

-- 3a. pr_registrar_log
CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_REGISTRAR_LOG" (
    p_tipo_evento  IN VARCHAR2,
    p_nivel        IN VARCHAR2,
    p_id_usuario   IN NUMBER   DEFAULT NULL,
    p_ip_origen    IN VARCHAR2 DEFAULT NULL,
    p_modulo       IN VARCHAR2,
    p_descripcion  IN VARCHAR2
) AS
    PRAGMA AUTONOMOUS_TRANSACTION;
BEGIN
    INSERT INTO log_eventos (
        id_log, tipo_evento, nivel, id_usuario, ip_origen, modulo, descripcion, fecha
    ) VALUES (
        seq_log_eventos.NEXTVAL, p_tipo_evento, p_nivel, p_id_usuario,
        p_ip_origen, p_modulo, p_descripcion, SYSTIMESTAMP
    );
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        NULL;
END pr_registrar_log;
/

-- 3b. fn_obtener_log_reciente
CREATE OR REPLACE EDITIONABLE FUNCTION "SEBAS_CASINO"."FN_OBTENER_LOG_RECIENTE" (
    p_cantidad IN NUMBER DEFAULT 100
) RETURN SYS_REFCURSOR AS
    cur SYS_REFCURSOR;
BEGIN
    OPEN cur FOR
        SELECT l.id_log, l.tipo_evento, l.nivel, l.id_usuario,
               u.username, l.ip_origen, l.modulo, l.descripcion, l.fecha
        FROM log_eventos l
        LEFT JOIN usuarios u ON l.id_usuario = u.id_usuario
        WHERE ROWNUM <= p_cantidad
        ORDER BY l.fecha DESC;
    RETURN cur;
END fn_obtener_log_reciente;
/

-- 3c. pr_registrar_apuesta
CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_REGISTRAR_APUESTA" (
    p_id_partida    IN  NUMBER,
    p_tipo_apuesta  IN  VARCHAR2,
    p_numero        IN  NUMBER DEFAULT NULL,
    p_monto         IN  NUMBER,
    p_multiplicador IN  NUMBER,
    p_ganancia      IN  NUMBER,
    p_resultado     IN  VARCHAR2,
    p_resultado_op  OUT VARCHAR2
) AS
BEGIN
    INSERT INTO apuestas (
        id_apuesta, id_partida, tipo_apuesta, numero_apuesta,
        monto, multiplicador, ganancia, resultado
    ) VALUES (
        seq_apuestas.NEXTVAL, p_id_partida, p_tipo_apuesta, p_numero,
        p_monto, p_multiplicador, p_ganancia, p_resultado
    );
    COMMIT;
    p_resultado_op := 'Guardado correctamente.';
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        p_resultado_op := 'Error al registrar apuesta: ' || SQLERRM;
END pr_registrar_apuesta;
/

-- ============================================================
-- 4. DATOS SEMILLA
-- ============================================================

INSERT INTO bonos (id_bono, nombre, tipo, valor, descripcion, activo)
    SELECT seq_bonos.NEXTVAL, 'Bono de Bienvenida', 'bienvenida', 50,
           'Credito inicial al registrarse', 1 FROM DUAL
    WHERE NOT EXISTS (SELECT 1 FROM bonos WHERE tipo = 'bienvenida');

INSERT INTO bonos (id_bono, nombre, tipo, valor, descripcion, activo)
    SELECT seq_bonos.NEXTVAL, 'Cashback Semanal', 'cashback', 10,
           'Devolucion del 10% de las perdidas netas de la semana', 1 FROM DUAL
    WHERE NOT EXISTS (SELECT 1 FROM bonos WHERE tipo = 'cashback');

INSERT INTO metodos_pago (id_metodo, tipo, descripcion, activo)
    SELECT seq_metodos_pago.NEXTVAL, 'PSE', 'Pago por PSE', 1 FROM DUAL
    WHERE NOT EXISTS (SELECT 1 FROM metodos_pago WHERE tipo = 'PSE');

INSERT INTO metodos_pago (id_metodo, tipo, descripcion, activo)
    SELECT seq_metodos_pago.NEXTVAL, 'Tarjeta', 'Tarjeta debito/credito', 1 FROM DUAL
    WHERE NOT EXISTS (SELECT 1 FROM metodos_pago WHERE tipo = 'Tarjeta');

INSERT INTO metodos_pago (id_metodo, tipo, descripcion, activo)
    SELECT seq_metodos_pago.NEXTVAL, 'Efectivo', 'Consignacion en efectivo', 1 FROM DUAL
    WHERE NOT EXISTS (SELECT 1 FROM metodos_pago WHERE tipo = 'Efectivo');

COMMIT;

-- ============================================================
-- 5. VISTAS
-- ============================================================

-- 5a. vw_bonos_usuario
CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_BONOS_USUARIO" ("ID_USUARIO_BONO", "ID_USUARIO", "USERNAME", "NOMBRE_USUARIO", "NOMBRE_BONO", "TIPO_BONO", "MONTO_APLICADO", "FECHA_APLICADO", "ESTADO", "DESCRIPCION") AS
  SELECT
    ub.id_usuario_bono, ub.id_usuario,
    u.username,
    u.nombre_1 || ' ' || u.apellido_1 AS nombre_usuario,
    b.nombre                           AS nombre_bono,
    b.tipo                             AS tipo_bono,
    ub.monto_aplicado,
    ub.fecha_aplicado,
    ub.estado,
    ub.descripcion
FROM usuario_bonos ub
JOIN usuarios u ON ub.id_usuario = u.id_usuario
JOIN bonos    b ON ub.id_bono    = b.id_bono
ORDER BY ub.fecha_aplicado DESC;

-- 5b. vw_depositos_pendientes
CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_DEPOSITOS_PENDIENTES" ("ID_DEPOSITO", "ID_USUARIO", "USERNAME", "NOMBRE", "MONTO", "ESTADO", "FECHA_SOLICITUD", "FECHA_PROCESAMIENTO", "METODO_PAGO", "DESCRIPCION") AS
  SELECT
    d.id_deposito, d.id_usuario,
    u.username, u.nombre_1 || ' ' || u.apellido_1 AS nombre,
    d.monto, d.estado, d.fecha_solicitud,
    d.fecha_procesamiento, m.tipo AS metodo_pago, d.descripcion
FROM depositos d
JOIN usuarios u ON d.id_usuario = u.id_usuario
LEFT JOIN metodos_pago m ON d.id_metodo = m.id_metodo
WHERE d.estado IN ('pendiente', 'procesando')
ORDER BY d.fecha_solicitud;

-- 5c. vw_retiros_pendientes
CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_RETIROS_PENDIENTES" ("ID_RETIRO", "ID_USUARIO", "USERNAME", "NOMBRE", "MONTO", "ESTADO", "FECHA_SOLICITUD", "FECHA_PROCESAMIENTO", "METODO_PAGO", "ADMIN_REVISOR") AS
  SELECT
    r.id_retiro, r.id_usuario,
    u.username, u.nombre_1 || ' ' || u.apellido_1 AS nombre,
    r.monto, r.estado, r.fecha_solicitud,
    r.fecha_procesamiento, m.tipo AS metodo_pago,
    ua.username AS admin_revisor
FROM retiros r
JOIN usuarios u ON r.id_usuario = u.id_usuario
LEFT JOIN metodos_pago m ON r.id_metodo = m.id_metodo
LEFT JOIN usuarios ua ON r.id_admin_revisor = ua.id_usuario
WHERE r.estado = 'pendiente'
ORDER BY r.fecha_solicitud;

-- 5d. vw_detalle_apuestas
CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_DETALLE_APUESTAS" ("ID_APUESTA", "ID_PARTIDA", "USERNAME", "NOMBRE_USUARIO", "JUEGO", "TIPO_APUESTA", "NUMERO_APUESTA", "MONTO", "MULTIPLICADOR", "GANANCIA", "RESULTADO", "FECHA") AS
  SELECT
    a.id_apuesta, a.id_partida,
    u.username,
    u.nombre_1 || ' ' || u.apellido_1 AS nombre_usuario,
    j.nombre                           AS juego,
    a.tipo_apuesta, a.numero_apuesta,
    a.monto, a.multiplicador, a.ganancia,
    a.resultado, p.fecha
FROM apuestas a
JOIN partidas p ON a.id_partida = p.id_partida
JOIN usuarios u ON p.id_usuario = u.id_usuario
JOIN juegos j   ON p.id_juego   = j.id_juego
ORDER BY p.fecha DESC;

-- ============================================================
-- 6. MODIFICACION: PKG_PARTIDAS (retorna id_partida)
-- ============================================================

CREATE OR REPLACE EDITIONABLE PACKAGE "SEBAS_CASINO"."PKG_PARTIDAS" AS

    PROCEDURE pr_registrar_partida(
        p_id_usuario IN  NUMBER,
        p_id_juego   IN  NUMBER,
        p_id_estado  IN  NUMBER,
        p_apuesta    IN  NUMBER,
        p_ganancia   IN  NUMBER,
        p_id_partida OUT NUMBER,
        p_msg        OUT VARCHAR2
    );

END PKG_PARTIDAS;
/

CREATE OR REPLACE EDITIONABLE PACKAGE BODY "SEBAS_CASINO"."PKG_PARTIDAS" AS

    PROCEDURE pr_registrar_partida(
        p_id_usuario IN  NUMBER,
        p_id_juego   IN  NUMBER,
        p_id_estado  IN  NUMBER,
        p_apuesta    IN  NUMBER,
        p_ganancia   IN  NUMBER,
        p_id_partida OUT NUMBER,
        p_msg        OUT VARCHAR2
    ) IS
    BEGIN
        IF PKG_USUARIOS.fn_usuario_puede_apostar(p_id_usuario, p_apuesta) = 0 THEN
            p_msg := 'Saldo insuficiente o usuario inactivo.';
            RETURN;
        END IF;

        INSERT INTO transacciones (
            id_transaccion, id_usuario, tipo,
            monto, fecha, descripcion
        ) VALUES (
            seq_transacciones.NEXTVAL, p_id_usuario, 'perdida',
            p_apuesta, CURRENT_TIMESTAMP,
            'Apuesta partida juego ' || p_id_juego
        );

        IF p_id_estado = 2 AND p_ganancia > 0 THEN
            INSERT INTO transacciones (
                id_transaccion, id_usuario, tipo,
                monto, fecha, descripcion
            ) VALUES (
                seq_transacciones.NEXTVAL, p_id_usuario, 'ganancia',
                p_ganancia, CURRENT_TIMESTAMP,
                'Ganancia partida juego ' || p_id_juego
            );
        END IF;

        INSERT INTO partidas (
            id_partida, id_usuario, id_juego,
            id_estado, fecha, apuesta,
            ganancia
        ) VALUES (
            seq_partidas.NEXTVAL, p_id_usuario, p_id_juego,
            p_id_estado, CURRENT_TIMESTAMP, p_apuesta,
            NVL(p_ganancia, 0)
        ) RETURNING id_partida INTO p_id_partida;

        COMMIT;
        p_msg := 'Guardado correctamente.';

    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            p_msg := 'Error al registrar partida: ' || SQLERRM;
    END pr_registrar_partida;

END PKG_PARTIDAS;
/

-- ============================================================
-- 7. EXTENDER PKG_USUARIOS (bonos, depositos, retiros)
-- ============================================================

CREATE OR REPLACE EDITIONABLE PACKAGE "SEBAS_CASINO"."PKG_USUARIOS" AS

    FUNCTION fn_total_depositado(
        p_id_usuario IN NUMBER
    ) RETURN NUMBER;

    FUNCTION fn_calcular_ganancia_neta(
        p_id_usuario IN NUMBER
    ) RETURN NUMBER;

    FUNCTION fn_usuario_puede_apostar(
        p_id_usuario IN NUMBER,
        p_monto      IN NUMBER
    ) RETURN NUMBER;

    PROCEDURE pr_realizar_deposito(
        p_id_usuario IN  NUMBER,
        p_monto      IN  NUMBER,
        p_resultado  OUT VARCHAR2
    );

    PROCEDURE pr_registrar_usuario(
        p_id_usuario     IN  NUMBER,
        p_username       IN  VARCHAR2,
        p_password       IN  VARCHAR2,
        p_nombre_1       IN  VARCHAR2,
        p_nombre_2       IN  VARCHAR2,
        p_apellido_1     IN  VARCHAR2,
        p_apellido_2     IN  VARCHAR2,
        p_correo         IN  VARCHAR2,
        p_fecha_nac      IN  DATE,
        p_id_rol         IN  NUMBER,
        p_resultado      OUT VARCHAR2
    );

    -- Bonos
    PROCEDURE pr_aplicar_bono(
        p_id_usuario  IN  NUMBER,
        p_id_bono     IN  NUMBER,
        p_monto       IN  NUMBER,
        p_descripcion IN  VARCHAR2 DEFAULT NULL,
        p_resultado   OUT VARCHAR2
    );

    PROCEDURE pr_cashback_semanal(
        p_resultado   OUT VARCHAR2
    );

    -- Depositos
    PROCEDURE pr_solicitar_deposito(
        p_id_usuario   IN  NUMBER,
        p_monto        IN  NUMBER,
        p_id_metodo    IN  NUMBER DEFAULT NULL,
        p_descripcion  IN  VARCHAR2 DEFAULT NULL,
        p_id_deposito  OUT NUMBER,
        p_resultado    OUT VARCHAR2
    );

    PROCEDURE pr_confirmar_deposito(
        p_id_deposito       IN  NUMBER,
        p_referencia_wompi  IN  VARCHAR2 DEFAULT NULL,
        p_resultado         OUT VARCHAR2
    );

    PROCEDURE pr_rechazar_deposito(
        p_id_deposito   IN  NUMBER,
        p_motivo        IN  VARCHAR2 DEFAULT NULL,
        p_resultado     OUT VARCHAR2
    );

    -- Retiros
    PROCEDURE pr_solicitar_retiro(
        p_id_usuario  IN  NUMBER,
        p_monto       IN  NUMBER,
        p_id_metodo   IN  NUMBER DEFAULT NULL,
        p_id_retiro   OUT NUMBER,
        p_resultado   OUT VARCHAR2
    );

    PROCEDURE pr_aprobar_retiro(
        p_id_retiro        IN  NUMBER,
        p_id_admin         IN  NUMBER,
        p_referencia_wompi IN  VARCHAR2 DEFAULT NULL,
        p_resultado        OUT VARCHAR2
    );

    PROCEDURE pr_rechazar_retiro(
        p_id_retiro   IN  NUMBER,
        p_motivo      IN  VARCHAR2 DEFAULT NULL,
        p_resultado   OUT VARCHAR2
    );

END PKG_USUARIOS;
/

CREATE OR REPLACE EDITIONABLE PACKAGE BODY "SEBAS_CASINO"."PKG_USUARIOS" AS

    FUNCTION fn_total_depositado(
        p_id_usuario IN NUMBER
    ) RETURN NUMBER IS
        v_total NUMBER(12,2);
    BEGIN
        SELECT NVL(SUM(monto), 0)
          INTO v_total
          FROM transacciones
         WHERE id_usuario = p_id_usuario
           AND tipo       = 'deposito';
        RETURN v_total;
    END fn_total_depositado;

    FUNCTION fn_calcular_ganancia_neta(
        p_id_usuario IN NUMBER
    ) RETURN NUMBER IS
        v_total_ganado   NUMBER(12,2);
        v_total_apostado NUMBER(12,2);
    BEGIN
        SELECT NVL(SUM(ganancia), 0),
               NVL(SUM(apuesta),  0)
          INTO v_total_ganado,
               v_total_apostado
          FROM partidas
         WHERE id_usuario = p_id_usuario;
        RETURN v_total_ganado - v_total_apostado;
    END fn_calcular_ganancia_neta;

    FUNCTION fn_usuario_puede_apostar(
        p_id_usuario IN NUMBER,
        p_monto      IN NUMBER
    ) RETURN NUMBER IS
        v_saldo  NUMBER(12,2);
        v_estado VARCHAR2(30);
    BEGIN
        SELECT saldo, estado
          INTO v_saldo, v_estado
          FROM vw_usuarios_detalle
         WHERE id_usuario = p_id_usuario;
        IF v_estado = 'activo' AND v_saldo >= p_monto THEN
            RETURN 1;
        ELSE
            RETURN 0;
        END IF;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN 0;
    END fn_usuario_puede_apostar;

    PROCEDURE pr_realizar_deposito(
        p_id_usuario IN  NUMBER,
        p_monto      IN  NUMBER,
        p_resultado  OUT VARCHAR2
    ) IS
        v_existe NUMBER;
    BEGIN
        IF p_monto <= 0 THEN
            p_resultado := 'El monto debe ser mayor a 0.';
            RETURN;
        END IF;
        SELECT COUNT(*) INTO v_existe FROM usuarios WHERE id_usuario = p_id_usuario;
        IF v_existe = 0 THEN
            p_resultado := 'Usuario no encontrado.';
            RETURN;
        END IF;
        INSERT INTO transacciones (
            id_transaccion, id_usuario, tipo, monto, fecha, descripcion
        ) VALUES (
            seq_transacciones.NEXTVAL, p_id_usuario, 'deposito',
            p_monto, CURRENT_TIMESTAMP, 'Recarga de saldo'
        );
        COMMIT;
        p_resultado := 'Deposito realizado correctamente.';
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            p_resultado := 'Error al realizar deposito: ' || SQLERRM;
    END pr_realizar_deposito;

    PROCEDURE pr_registrar_usuario(
        p_id_usuario     IN  NUMBER,
        p_username       IN  VARCHAR2,
        p_password       IN  VARCHAR2,
        p_nombre_1       IN  VARCHAR2,
        p_nombre_2       IN  VARCHAR2,
        p_apellido_1     IN  VARCHAR2,
        p_apellido_2     IN  VARCHAR2,
        p_correo         IN  VARCHAR2,
        p_fecha_nac      IN  DATE,
        p_id_rol         IN  NUMBER,
        p_resultado      OUT VARCHAR2
    ) IS
        v_id_estado NUMBER;
    BEGIN
        SELECT id_estado INTO v_id_estado FROM estado_usuario WHERE nombre = 'activo';
        INSERT INTO usuarios (
            id_usuario, username, password,
            nombre_1, nombre_2,
            apellido_1, apellido_2,
            correo, fecha_nacimiento,
            saldo, id_rol, id_estado, fecha_registro
        ) VALUES (
            p_id_usuario, p_username, p_password,
            p_nombre_1, p_nombre_2,
            p_apellido_1, p_apellido_2,
            p_correo, p_fecha_nac,
            0, p_id_rol, v_id_estado, SYSDATE
        );
        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN DUP_VAL_ON_INDEX THEN
            ROLLBACK;
            p_resultado := 'El username o correo ya estan registrados.';
        WHEN OTHERS THEN
            ROLLBACK;
            p_resultado := 'Error al registrar usuario: ' || SQLERRM;
    END pr_registrar_usuario;

    -- ===== BONOS =====

    PROCEDURE pr_aplicar_bono(
        p_id_usuario  IN  NUMBER,
        p_id_bono     IN  NUMBER,
        p_monto       IN  NUMBER,
        p_descripcion IN  VARCHAR2 DEFAULT NULL,
        p_resultado   OUT VARCHAR2
    ) AS
        v_ya_tiene  NUMBER := 0;
    BEGIN
        SELECT COUNT(*) INTO v_ya_tiene
        FROM usuario_bonos
        WHERE id_usuario = p_id_usuario
          AND id_bono    = p_id_bono
          AND TRUNC(fecha_aplicado) = TRUNC(SYSDATE);

        IF v_ya_tiene > 0 THEN
            p_resultado := 'El bono ya fue aplicado hoy.';
            RETURN;
        END IF;

        INSERT INTO usuario_bonos (
            id_usuario_bono, id_usuario, id_bono, monto_aplicado,
            fecha_aplicado, estado, descripcion
        ) VALUES (
            seq_usuario_bonos.NEXTVAL, p_id_usuario, p_id_bono, p_monto,
            SYSTIMESTAMP, 'aplicado', p_descripcion
        );

        UPDATE usuarios SET saldo = saldo + p_monto WHERE id_usuario = p_id_usuario;

        INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
        VALUES (seq_transacciones.NEXTVAL, p_id_usuario, 'bono', p_monto,
                SYSTIMESTAMP, NVL(p_descripcion, 'Bono aplicado'));

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            p_resultado := 'Error al aplicar bono: ' || SQLERRM;
    END pr_aplicar_bono;

    PROCEDURE pr_cashback_semanal(
        p_resultado   OUT VARCHAR2
    ) AS
        v_id_bono    NUMBER;
        v_monto      NUMBER;
        v_procesados NUMBER := 0;
    BEGIN
        SELECT id_bono INTO v_id_bono
        FROM bonos WHERE tipo = 'cashback' AND activo = 1 AND ROWNUM = 1;

        FOR usr IN (
            SELECT u.id_usuario
            FROM usuarios u
            JOIN estado_usuario eu ON u.id_estado = eu.id_estado
            WHERE eu.nombre = 'activo' AND u.id_rol = 2
        ) LOOP
            v_monto := fn_calcular_cashback(usr.id_usuario);
            IF v_monto > 0 THEN
                pr_aplicar_bono(usr.id_usuario, v_id_bono, v_monto,
                                'Cashback semanal automatico', p_resultado);
                v_procesados := v_procesados + 1;
            END IF;
        END LOOP;
        COMMIT;
        p_resultado := 'Cashback procesado para ' || v_procesados || ' usuarios.';
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error en cashback semanal: ' || SQLERRM;
    END pr_cashback_semanal;

    -- ===== DEPOSITOS =====

    PROCEDURE pr_solicitar_deposito(
        p_id_usuario   IN  NUMBER,
        p_monto        IN  NUMBER,
        p_id_metodo    IN  NUMBER DEFAULT NULL,
        p_descripcion  IN  VARCHAR2 DEFAULT NULL,
        p_id_deposito  OUT NUMBER,
        p_resultado    OUT VARCHAR2
    ) AS
    BEGIN
        INSERT INTO depositos (
            id_deposito, id_usuario, id_metodo, monto, estado,
            fecha_solicitud, descripcion
        ) VALUES (
            seq_depositos.NEXTVAL, p_id_usuario, p_id_metodo, p_monto,
            'pendiente', SYSTIMESTAMP, p_descripcion
        ) RETURNING id_deposito INTO p_id_deposito;

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            p_id_deposito := -1;
            p_resultado := 'Error al crear solicitud: ' || SQLERRM;
    END pr_solicitar_deposito;

    PROCEDURE pr_confirmar_deposito(
        p_id_deposito       IN  NUMBER,
        p_referencia_wompi  IN  VARCHAR2 DEFAULT NULL,
        p_resultado         OUT VARCHAR2
    ) AS
        v_id_usuario  NUMBER;
        v_monto       NUMBER;
        v_estado      VARCHAR2(20);
    BEGIN
        SELECT id_usuario, monto, estado
        INTO v_id_usuario, v_monto, v_estado
        FROM depositos WHERE id_deposito = p_id_deposito
        FOR UPDATE;

        IF v_estado NOT IN ('pendiente', 'procesando') THEN
            p_resultado := 'El deposito no esta en estado procesable.';
            RETURN;
        END IF;

        UPDATE depositos SET
            estado              = 'aprobado',
            fecha_procesamiento = SYSTIMESTAMP,
            referencia_wompi    = p_referencia_wompi
        WHERE id_deposito = p_id_deposito;

        UPDATE usuarios SET saldo = saldo + v_monto WHERE id_usuario = v_id_usuario;

        INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
        VALUES (seq_transacciones.NEXTVAL, v_id_usuario, 'deposito', v_monto,
                SYSTIMESTAMP, 'Deposito aprobado. Ref Wompi: ' || NVL(p_referencia_wompi, 'N/A'));

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN NO_DATA_FOUND THEN ROLLBACK; p_resultado := 'Deposito no encontrado.';
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error al confirmar deposito: ' || SQLERRM;
    END pr_confirmar_deposito;

    PROCEDURE pr_rechazar_deposito(
        p_id_deposito   IN  NUMBER,
        p_motivo        IN  VARCHAR2 DEFAULT NULL,
        p_resultado     OUT VARCHAR2
    ) AS
    BEGIN
        UPDATE depositos SET
            estado              = 'rechazado',
            fecha_procesamiento = SYSTIMESTAMP,
            error_detalle       = p_motivo
        WHERE id_deposito = p_id_deposito AND estado IN ('pendiente', 'procesando');

        IF SQL%ROWCOUNT = 0 THEN
            p_resultado := 'Deposito no encontrado o no procesable.';
        ELSE
            COMMIT;
            p_resultado := 'Guardado correctamente.';
        END IF;
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error: ' || SQLERRM;
    END pr_rechazar_deposito;

    -- ===== RETIROS =====

    PROCEDURE pr_solicitar_retiro(
        p_id_usuario  IN  NUMBER,
        p_monto       IN  NUMBER,
        p_id_metodo   IN  NUMBER DEFAULT NULL,
        p_id_retiro   OUT NUMBER,
        p_resultado   OUT VARCHAR2
    ) AS
        v_saldo NUMBER;
    BEGIN
        SELECT saldo INTO v_saldo FROM usuarios
        WHERE id_usuario = p_id_usuario FOR UPDATE;

        IF v_saldo < p_monto THEN
            p_resultado := 'Saldo insuficiente para el retiro.';
            RETURN;
        END IF;

        UPDATE usuarios SET saldo = saldo - p_monto WHERE id_usuario = p_id_usuario;

        INSERT INTO retiros (
            id_retiro, id_usuario, id_metodo, monto, estado, fecha_solicitud
        ) VALUES (
            seq_retiros.NEXTVAL, p_id_usuario, p_id_metodo, p_monto,
            'pendiente', SYSTIMESTAMP
        ) RETURNING id_retiro INTO p_id_retiro;

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            p_id_retiro := -1;
            p_resultado := 'Error al solicitar retiro: ' || SQLERRM;
    END pr_solicitar_retiro;

    PROCEDURE pr_aprobar_retiro(
        p_id_retiro        IN  NUMBER,
        p_id_admin         IN  NUMBER,
        p_referencia_wompi IN  VARCHAR2 DEFAULT NULL,
        p_resultado        OUT VARCHAR2
    ) AS
        v_id_usuario NUMBER;
        v_monto      NUMBER;
    BEGIN
        SELECT id_usuario, monto INTO v_id_usuario, v_monto
        FROM retiros WHERE id_retiro = p_id_retiro AND estado = 'pendiente'
        FOR UPDATE;

        UPDATE retiros SET
            estado              = 'aprobado',
            fecha_procesamiento = SYSTIMESTAMP,
            id_admin_revisor    = p_id_admin,
            referencia_wompi    = p_referencia_wompi
        WHERE id_retiro = p_id_retiro;

        INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
        VALUES (seq_transacciones.NEXTVAL, v_id_usuario, 'retiro', v_monto,
                SYSTIMESTAMP, 'Retiro aprobado. Ref: ' || NVL(p_referencia_wompi, 'N/A'));

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN NO_DATA_FOUND THEN ROLLBACK; p_resultado := 'Retiro no encontrado o ya procesado.';
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error: ' || SQLERRM;
    END pr_aprobar_retiro;

    PROCEDURE pr_rechazar_retiro(
        p_id_retiro   IN  NUMBER,
        p_motivo      IN  VARCHAR2 DEFAULT NULL,
        p_resultado   OUT VARCHAR2
    ) AS
        v_id_usuario NUMBER;
        v_monto      NUMBER;
    BEGIN
        SELECT id_usuario, monto INTO v_id_usuario, v_monto
        FROM retiros WHERE id_retiro = p_id_retiro AND estado = 'pendiente'
        FOR UPDATE;

        UPDATE retiros SET
            estado              = 'rechazado',
            fecha_procesamiento = SYSTIMESTAMP,
            motivo_rechazo      = p_motivo
        WHERE id_retiro = p_id_retiro;

        UPDATE usuarios SET saldo = saldo + v_monto WHERE id_usuario = v_id_usuario;

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN NO_DATA_FOUND THEN ROLLBACK; p_resultado := 'Retiro no encontrado.';
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error: ' || SQLERRM;
    END pr_rechazar_retiro;

END PKG_USUARIOS;
/

PROMPT 'mejoras_casino_v2.sql ejecutado correctamente.'
