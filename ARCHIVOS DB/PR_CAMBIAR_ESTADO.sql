--------------------------------------------------------
--  DDL for Procedure PR_CAMBIAR_ESTADO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_CAMBIAR_ESTADO" (
    p_id_usuario     IN  NUMBER,
    p_nuevo_estado   IN  VARCHAR2,
    p_resultado      OUT VARCHAR2
) AS
BEGIN
    UPDATE usuarios SET
        id_estado = (SELECT id_estado FROM estado_usuario WHERE nombre = p_nuevo_estado)
    WHERE id_usuario = p_id_usuario;

    IF SQL%ROWCOUNT = 0 THEN
        p_resultado := 'Usuario no encontrado.';
    ELSE
        COMMIT;
        p_resultado := 'Guardado correctamente.';
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        p_resultado := 'Error al cambiar estado: ' || SQLERRM;
END pr_cambiar_estado;

/
