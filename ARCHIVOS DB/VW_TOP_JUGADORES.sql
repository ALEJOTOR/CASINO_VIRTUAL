--------------------------------------------------------
--  DDL for View VW_TOP_JUGADORES
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_TOP_JUGADORES" ("ID_USUARIO", "USERNAME", "NOMBRE_COMPLETO", "SALDO", "TOTAL_PARTIDAS", "PARTIDAS_GANADAS", "PARTIDAS_PERDIDAS", "TOTAL_APOSTADO", "TOTAL_GANADO", "GANANCIA_NETA") AS 
  SELECT
    u.id_usuario,
    u.username,
    u.nombre_1 || ' ' || u.apellido_1              AS nombre_completo,
    u.saldo,
    COUNT(p.id_partida)                              AS total_partidas,
    SUM(CASE WHEN p.id_estado = 2 THEN 1 ELSE 0 END) AS partidas_ganadas,
    SUM(CASE WHEN p.id_estado = 3 THEN 1 ELSE 0 END) AS partidas_perdidas,
    NVL(SUM(p.apuesta), 0)                           AS total_apostado,
    NVL(SUM(p.ganancia), 0)                          AS total_ganado,
    NVL(SUM(p.ganancia), 0) - NVL(SUM(p.apuesta), 0) AS ganancia_neta
FROM usuarios u
JOIN partidas p ON u.id_usuario = p.id_usuario
WHERE u.id_rol != 1
GROUP BY u.id_usuario, u.username, u.nombre_1, u.apellido_1, u.saldo
ORDER BY total_apostado DESC
;
