-- ============================================================
-- MIGRACIÓN: Eliminar columna resultado de la tabla partidas
-- ============================================================
-- Ejecutar como administrador de la base de datos Oracle
-- ============================================================

ALTER TABLE partidas DROP COLUMN resultado;

-- Si existe la vista vw_historial_partidas, recrearla sin resultado
CREATE OR REPLACE VIEW vw_historial_partidas AS
SELECT p.id_partida,
       p.id_usuario,
       j.nombre  AS nombre_juego,
       ep.nombre_estado AS estado,
       p.fecha,
       p.apuesta,
       p.ganancia
  FROM partidas p
  JOIN juegos j ON p.id_juego = j.id_juego
  JOIN estado_partidas ep ON p.id_estado = ep.id_estado;

-- Si existe el procedimiento PKG_PARTIDAS.pr_registrar_partida,
-- debe recrearse eliminando el parámetro p_resultado
