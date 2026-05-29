--------------------------------------------------------
--  DDL for View VW_BONOS_USUARIO
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_BONOS_USUARIO" ("ID_USUARIO_BONO", "ID_USUARIO", "USERNAME", "NOMBRE_USUARIO", "NOMBRE_BONO", "TIPO_BONO", "MONTO_APLICADO", "FECHA_APLICADO", "ESTADO", "DESCRIPCION") AS 
  SELECT
    ub.id_usuario_bono, ub.id_usuario,
    u.username,
    u.nombre_1 || ' ' || u.apellido_1 AS nombre_usuario,
    b.nombre                           AS nombre_bono,
    b.tipo                             AS tipo_bono,
    ub.monto_aplicado,
    ub.fecha_aplicado,
    ub.estado,
    ub.descripcion
FROM usuario_bonos ub
JOIN usuarios u ON ub.id_usuario = u.id_usuario
JOIN bonos    b ON ub.id_bono    = b.id_bono
ORDER BY ub.fecha_aplicado DESC
;
