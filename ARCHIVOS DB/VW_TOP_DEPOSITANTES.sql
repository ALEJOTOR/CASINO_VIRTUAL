--------------------------------------------------------
--  DDL for View VW_TOP_DEPOSITANTES
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_TOP_DEPOSITANTES" ("ID_USUARIO", "NOMBRE_COMPLETO", "USERNAME", "TOTAL_DEPOSITADO", "NUM_DEPOSITOS") AS 
  SELECT
    u.id_usuario,
    u.nombre_1 || ' ' || u.apellido_1   AS nombre_completo,
    u.username,
    NVL(SUM(t.monto), 0)                 AS total_depositado,
    COUNT(t.id_transaccion)              AS num_depositos
FROM usuarios u
JOIN transacciones t ON u.id_usuario = t.id_usuario
WHERE u.id_rol != 1 AND t.tipo = 'deposito'
GROUP BY u.id_usuario, u.nombre_1, u.apellido_1, u.username
ORDER BY total_depositado DESC
;
