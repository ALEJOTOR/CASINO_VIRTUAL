--------------------------------------------------------
--  DDL for Procedure PR_SOLICITAR_RETIRO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_SOLICITAR_RETIRO" (
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

/
