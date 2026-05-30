--------------------------------------------------------
--  DDL for View VW_DATOS_BANCARIOS
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_DATOS_BANCARIOS" ("ID_DATOS_BANCARIOS", "ID_USUARIO", "USERNAME", "BANCO_ID", "TIPO_CUENTA", "NUMERO_CUENTA", "TIPO_DOC", "NUMERO_DOC", "NOMBRE_TITULAR", "ACTIVO", "FECHA_REGISTRO") AS 
  SELECT
    db.id_datos_bancarios,
    db.id_usuario,
    u.username,
    db.banco_id,
    db.tipo_cuenta,
    db.numero_cuenta,
    db.tipo_doc,
    db.numero_doc,
    db.nombre_titular,
    db.activo,
    db.fecha_registro
FROM datos_bancarios_usuario db
JOIN usuarios u ON db.id_usuario = u.id_usuario
;
