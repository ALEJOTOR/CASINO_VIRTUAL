--------------------------------------------------------
--  DDL for Procedure PR_SOLICITAR_DEPOSITO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_SOLICITAR_DEPOSITO" (
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

/
