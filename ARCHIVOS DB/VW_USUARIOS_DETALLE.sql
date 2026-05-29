--------------------------------------------------------
--  DDL for View VW_USUARIOS_DETALLE
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_USUARIOS_DETALLE" ("ID_USUARIO", "USERNAME", "PASSWORD", "NOMBRE_1", "NOMBRE_2", "APELLIDO_1", "APELLIDO_2", "CORREO", "FECHA_NACIMIENTO", "SALDO", "ID_ROL", "FECHA_REGISTRO", "ESTADO") AS 
  SELECT
        u.id_usuario,
        u.username,
        u.password,
        u.nombre_1,
        u.nombre_2,
        u.apellido_1,
        u.apellido_2,
        u.correo,
        u.fecha_nacimiento,
        u.saldo,
        u.id_rol,
        u.fecha_registro,
        e.nombre AS estado        -- nombre legible, no el id_estado numérico
    FROM  usuarios     u
    JOIN  estado_usuario e ON u.id_estado = e.id_estado
;
