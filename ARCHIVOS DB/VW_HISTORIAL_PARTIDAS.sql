--------------------------------------------------------
--  DDL for View VW_HISTORIAL_PARTIDAS
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_HISTORIAL_PARTIDAS" ("ID_PARTIDA", "ID_USUARIO", "NOMBRE_JUEGO", "ESTADO", "FECHA", "APUESTA", "GANANCIA") AS 
  SELECT p.id_partida,
       p.id_usuario,
       j.nombre  AS nombre_juego,
       ep.nombre_estado AS estado,
       p.fecha,
       p.apuesta,
       p.ganancia
  FROM partidas p
  JOIN juegos j ON p.id_juego = j.id_juego
  JOIN estado_partidas ep ON p.id_estado = ep.id_estado
;
