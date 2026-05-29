--------------------------------------------------------
--  DDL for Procedure PR_REALIZAR_RETIRO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_REALIZAR_RETIRO" (
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

    -- NOTA: el trigger trg_actualizar_saldo actualiza el saldo al insertar en transacciones
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
END pr_realizar_retiro;

/
