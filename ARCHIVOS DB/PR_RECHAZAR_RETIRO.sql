--------------------------------------------------------
--  DDL for Procedure PR_RECHAZAR_RETIRO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_RECHAZAR_RETIRO" (
    p_id_retiro   IN  NUMBER,
    p_id_admin    IN  NUMBER,
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
        id_admin_revisor    = p_id_admin,
        motivo_rechazo      = p_motivo
    WHERE id_retiro = p_id_retiro;

    UPDATE usuarios SET saldo = saldo + v_monto WHERE id_usuario = v_id_usuario;

    COMMIT;
    p_resultado := 'Guardado correctamente.';
EXCEPTION
    WHEN NO_DATA_FOUND THEN ROLLBACK; p_resultado := 'Retiro no encontrado.';
    WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error: ' || SQLERRM;
END pr_rechazar_retiro;

/
