--------------------------------------------------------
--  DDL for View VW_DETALLE_APUESTAS
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_DETALLE_APUESTAS" ("ID_APUESTA", "ID_PARTIDA", "USERNAME", "NOMBRE_USUARIO", "JUEGO", "TIPO_APUESTA", "NUMERO_APUESTA", "MONTO", "MULTIPLICADOR", "GANANCIA", "RESULTADO", "FECHA") AS 
  SELECT
    a.id_apuesta, a.id_partida,
    u.username,
    u.nombre_1 || ' ' || u.apellido_1 AS nombre_usuario,
    j.nombre                           AS juego,
    a.tipo_apuesta, a.numero_apuesta,
    a.monto, a.multiplicador, a.ganancia,
    a.resultado, p.fecha
FROM apuestas a
JOIN partidas p ON a.id_partida = p.id_partida
JOIN usuarios u ON p.id_usuario = u.id_usuario
JOIN juegos j   ON p.id_juego   = j.id_juego
ORDER BY p.fecha DESC
;
