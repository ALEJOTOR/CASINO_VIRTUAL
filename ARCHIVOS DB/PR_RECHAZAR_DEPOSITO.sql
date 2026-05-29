--------------------------------------------------------
--  DDL for Procedure PR_RECHAZAR_DEPOSITO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_RECHAZAR_DEPOSITO" (
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

/
