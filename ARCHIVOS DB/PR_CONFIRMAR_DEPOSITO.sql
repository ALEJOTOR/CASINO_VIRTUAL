--------------------------------------------------------
--  DDL for Procedure PR_CONFIRMAR_DEPOSITO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_CONFIRMAR_DEPOSITO" (
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

    -- NOTA: el trigger trg_actualizar_saldo actualiza el saldo al insertar en transacciones
    INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
    VALUES (seq_transacciones.NEXTVAL, v_id_usuario, 'deposito', v_monto,
            SYSTIMESTAMP, 'Deposito aprobado. Ref Wompi: ' || NVL(p_referencia_wompi, 'N/A'));

    COMMIT;
    p_resultado := 'Guardado correctamente.';
EXCEPTION
    WHEN NO_DATA_FOUND THEN ROLLBACK; p_resultado := 'Deposito no encontrado.';
    WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error al confirmar deposito: ' || SQLERRM;
END pr_confirmar_deposito;

/
