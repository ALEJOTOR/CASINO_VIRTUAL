--------------------------------------------------------
--  DDL for Procedure PR_APROBAR_RETIRO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_APROBAR_RETIRO" (
    p_id_retiro        IN  NUMBER,
    p_id_admin         IN  NUMBER DEFAULT NULL,   -- NULL = aprobación automática por Wompi Payouts
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
        id_admin_revisor    = p_id_admin,       -- NULL para aprobación automática
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

/
