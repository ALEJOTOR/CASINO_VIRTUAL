-- ============================================================
-- FIX: Eliminar doble actualizacion de saldo en stored procedures
-- El trigger trg_actualizar_saldo ya maneja el saldo al
-- insertar en transacciones. Los SPs NO deben hacer UPDATE
-- directo a usuarios.saldo cuando tambien insertan en transacciones.
-- ============================================================

-- 1. PR_CONFIRMAR_DEPOSITO: tenia UPDATE saldo + INSERT transacciones 'deposito'
CREATE OR REPLACE EDITIONABLE PROCEDURE PR_CONFIRMAR_DEPOSITO (
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

    -- El trigger trg_actualizar_saldo actualiza el saldo (+)
    INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
    VALUES (seq_transacciones.NEXTVAL, v_id_usuario, 'deposito', v_monto,
            SYSTIMESTAMP, 'Deposito aprobado. Ref Wompi: ' || NVL(p_referencia_wompi, 'N/A'));

    COMMIT;
    p_resultado := 'Guardado correctamente.';
EXCEPTION
    WHEN NO_DATA_FOUND THEN ROLLBACK; p_resultado := 'Deposito no encontrado.';
    WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error al confirmar deposito: ' || SQLERRM;
END PR_CONFIRMAR_DEPOSITO;
/

-- 2. PR_REALIZAR_RETIRO: tenia INSERT transacciones 'retiro' + UPDATE saldo
CREATE OR REPLACE EDITIONABLE PROCEDURE PR_REALIZAR_RETIRO (
    p_id_usuario IN  NUMBER,
    p_monto      IN  NUMBER,
    p_resultado  OUT VARCHAR2
) AS
    v_saldo_actual usuarios.saldo%TYPE;
BEGIN
    SELECT saldo INTO v_saldo_actual
    FROM usuarios
    WHERE id_usuario = p_id_usuario
    FOR UPDATE;

    IF v_saldo_actual < p_monto THEN
        p_resultado := 'Saldo insuficiente.';
        RETURN;
    END IF;

    -- El trigger trg_actualizar_saldo actualiza el saldo (-)
    INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
    VALUES (seq_transacciones.NEXTVAL, p_id_usuario, 'retiro', p_monto, CURRENT_TIMESTAMP,
            'Retiro de saldo');

    COMMIT;
    p_resultado := 'Guardado correctamente.';
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        p_resultado := 'Usuario no encontrado.';
    WHEN OTHERS THEN
        ROLLBACK;
        p_resultado := 'Error al realizar retiro: ' || SQLERRM;
END PR_REALIZAR_RETIRO;
/

-- 3. PR_APLICAR_BONO: tenia UPDATE saldo + INSERT transacciones 'bono'
--    Nota: el trigger necesita manejar tipo 'bono' (se actualiza abajo)
CREATE OR REPLACE EDITIONABLE PROCEDURE PR_APLICAR_BONO (
    p_id_usuario    IN  NUMBER,
    p_id_bono       IN  NUMBER,
    p_monto         IN  NUMBER,
    p_descripcion   IN  VARCHAR2 DEFAULT NULL,
    p_resultado     OUT VARCHAR2
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

    -- El trigger trg_actualizar_saldo actualiza el saldo (+)
    INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
    VALUES (seq_transacciones.NEXTVAL, p_id_usuario, 'bono', p_monto,
            SYSTIMESTAMP, NVL(p_descripcion, 'Bono aplicado'));

    COMMIT;
    p_resultado := 'Guardado correctamente.';
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        p_resultado := 'Error al aplicar bono: ' || SQLERRM;
END PR_APLICAR_BONO;
/

-- 4. Modificar trigger para que tambien maneje tipo 'bono'
CREATE OR REPLACE TRIGGER trg_actualizar_saldo
AFTER INSERT ON transacciones
FOR EACH ROW
BEGIN
    IF :NEW.tipo IN ('deposito', 'ganancia', 'bono') THEN
        UPDATE usuarios
           SET saldo = saldo + :NEW.monto
         WHERE id_usuario = :NEW.id_usuario;

    ELSIF :NEW.tipo IN ('perdida', 'retiro') THEN
        UPDATE usuarios
           SET saldo = saldo - :NEW.monto
         WHERE id_usuario = :NEW.id_usuario;
    END IF;
END;
/

COMMIT;
