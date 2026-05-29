--------------------------------------------------------
--  DDL for View VW_RETIROS_PENDIENTES
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_RETIROS_PENDIENTES" ("ID_RETIRO", "ID_USUARIO", "USERNAME", "NOMBRE", "MONTO", "ESTADO", "FECHA_SOLICITUD", "FECHA_PROCESAMIENTO", "METODO_PAGO", "ADMIN_REVISOR") AS 
  SELECT
    r.id_retiro, r.id_usuario,
    u.username, u.nombre_1 || ' ' || u.apellido_1 AS nombre,
    r.monto, r.estado, r.fecha_solicitud,
    r.fecha_procesamiento, m.tipo AS metodo_pago,
    ua.username AS admin_revisor
FROM retiros r
JOIN usuarios u ON r.id_usuario = u.id_usuario
LEFT JOIN metodos_pago m ON r.id_metodo = m.id_metodo
LEFT JOIN usuarios ua ON r.id_admin_revisor = ua.id_usuario
WHERE r.estado = 'pendiente'
ORDER BY r.fecha_solicitud
;
