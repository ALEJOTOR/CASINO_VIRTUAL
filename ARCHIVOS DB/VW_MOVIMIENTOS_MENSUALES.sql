--------------------------------------------------------
--  DDL for View VW_MOVIMIENTOS_MENSUALES
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_MOVIMIENTOS_MENSUALES" ("MES", "DEPOSITOS", "RETIROS") AS 
  WITH t_na AS (
    SELECT t.* FROM transacciones t
     WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = t.id_usuario AND u.id_rol = 1)
),
meses AS (
    SELECT ADD_MONTHS(TRUNC(SYSDATE, 'MM'), -(LEVEL - 1)) AS mes
    FROM DUAL CONNECT BY LEVEL <= 12
)
SELECT
    TO_CHAR(m.mes, 'MON YY') AS mes,
    NVL((SELECT SUM(monto) FROM t_na WHERE tipo = 'deposito' AND TRUNC(fecha, 'MM') = m.mes), 0) AS depositos,
    NVL((SELECT SUM(monto) FROM t_na WHERE tipo = 'retiro' AND TRUNC(fecha, 'MM') = m.mes), 0)   AS retiros
FROM meses m
ORDER BY m.mes
;
