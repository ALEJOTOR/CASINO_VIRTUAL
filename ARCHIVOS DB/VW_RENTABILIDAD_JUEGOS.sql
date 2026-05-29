--------------------------------------------------------
--  DDL for View VW_RENTABILIDAD_JUEGOS
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_RENTABILIDAD_JUEGOS" ("JUEGO", "TOTAL_PARTIDAS", "TOTAL_APOSTADO", "GANANCIA_CASA", "MARGEN_PORCENTAJE") AS 
  WITH p_na AS (
    SELECT p.* FROM partidas p
     WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = p.id_usuario AND u.id_rol = 1)
)
SELECT
    j.nombre                                   AS juego,
    COUNT(p.id_partida)                         AS total_partidas,
    NVL(SUM(p.apuesta), 0)                      AS total_apostado,
    NVL(SUM(p.apuesta - p.ganancia), 0)         AS ganancia_casa,
    ROUND(
        CASE WHEN NVL(SUM(p.apuesta), 0) > 0
             THEN (SUM(p.apuesta - p.ganancia) / SUM(p.apuesta)) * 100
             ELSE 0
        END, 2
    )                                           AS margen_porcentaje
FROM juegos j
LEFT JOIN p_na p ON j.id_juego = p.id_juego
GROUP BY j.id_juego, j.nombre
ORDER BY ganancia_casa DESC
;
