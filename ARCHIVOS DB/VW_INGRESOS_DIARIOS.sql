--------------------------------------------------------
--  DDL for View VW_INGRESOS_DIARIOS
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_INGRESOS_DIARIOS" ("FECHA", "DEPOSITOS", "GANANCIA_CASA") AS 
  WITH p_na AS (
    SELECT p.* FROM partidas p
     WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = p.id_usuario AND u.id_rol = 1)
),
t_na AS (
    SELECT t.* FROM transacciones t
     WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = t.id_usuario AND u.id_rol = 1)
),
dias AS (
    SELECT TRUNC(SYSDATE) - LEVEL + 1 AS fecha FROM DUAL CONNECT BY LEVEL <= 30
)
SELECT
    d.fecha,
    NVL((SELECT SUM(monto) FROM t_na WHERE tipo = 'deposito' AND TRUNC(fecha) = d.fecha), 0) AS depositos,
    NVL((SELECT SUM(apuesta - ganancia) FROM p_na WHERE TRUNC(fecha) = d.fecha), 0)           AS ganancia_casa
FROM dias d
ORDER BY d.fecha
;
