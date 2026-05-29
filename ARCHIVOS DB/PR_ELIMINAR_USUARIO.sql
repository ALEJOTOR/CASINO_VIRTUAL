--------------------------------------------------------
--  DDL for Procedure PR_ELIMINAR_USUARIO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_ELIMINAR_USUARIO" (
    p_id_usuario IN  NUMBER,
    p_resultado  OUT VARCHAR2
) AS
BEGIN
    DELETE FROM usuarios WHERE id_usuario = p_id_usuario;

    IF SQL%ROWCOUNT = 0 THEN
        p_resultado := 'Usuario no encontrado.';
    ELSE
        COMMIT;
        p_resultado := 'Guardado correctamente.';
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        p_resultado := 'Error al eliminar usuario: ' || SQLERRM;
END pr_eliminar_usuario;

/
