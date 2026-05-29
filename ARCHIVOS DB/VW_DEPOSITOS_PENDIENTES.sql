--------------------------------------------------------
--  DDL for View VW_DEPOSITOS_PENDIENTES
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_DEPOSITOS_PENDIENTES" ("ID_DEPOSITO", "ID_USUARIO", "USERNAME", "NOMBRE", "MONTO", "ESTADO", "FECHA_SOLICITUD", "FECHA_PROCESAMIENTO", "METODO_PAGO", "DESCRIPCION") AS 
  SELECT
    d.id_deposito, d.id_usuario,
    u.username, u.nombre_1 || ' ' || u.apellido_1 AS nombre,
    d.monto, d.estado, d.fecha_solicitud,
    d.fecha_procesamiento, m.tipo AS metodo_pago, d.descripcion
FROM depositos d
JOIN usuarios u ON d.id_usuario = u.id_usuario
LEFT JOIN metodos_pago m ON d.id_metodo = m.id_metodo
WHERE d.estado IN ('pendiente', 'procesando')
ORDER BY d.fecha_solicitud
;
