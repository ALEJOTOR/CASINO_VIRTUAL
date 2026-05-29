--------------------------------------------------------
--  DDL for Procedure PR_ACTUALIZAR_USUARIO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_ACTUALIZAR_USUARIO" (
    p_id_usuario     IN  NUMBER,
    p_username       IN  VARCHAR2,
    p_password       IN  VARCHAR2,
    p_nombre_1       IN  VARCHAR2,
    p_nombre_2       IN  VARCHAR2 DEFAULT NULL,
    p_apellido_1     IN  VARCHAR2,
    p_apellido_2     IN  VARCHAR2 DEFAULT NULL,
    p_correo         IN  VARCHAR2,
    p_fecha_nac      IN  DATE,
    p_id_rol         IN  NUMBER,
    p_estado         IN  VARCHAR2,
    p_resultado      OUT VARCHAR2
) AS
BEGIN
    UPDATE usuarios SET
        username         = p_username,
        password         = p_password,
        nombre_1         = p_nombre_1,
        nombre_2         = p_nombre_2,
        apellido_1       = p_apellido_1,
        apellido_2       = p_apellido_2,
        correo           = p_correo,
        fecha_nacimiento = p_fecha_nac,
        id_rol           = p_id_rol,
        id_estado        = (SELECT id_estado FROM estado_usuario WHERE nombre = p_estado)
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
        p_resultado := 'Error al actualizar usuario: ' || SQLERRM;
END pr_actualizar_usuario;

/
