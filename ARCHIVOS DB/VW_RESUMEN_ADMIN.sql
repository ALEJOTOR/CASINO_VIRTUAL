--------------------------------------------------------
--  DDL for View VW_RESUMEN_ADMIN
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "SEBAS_CASINO"."VW_RESUMEN_ADMIN" ("TOTAL_USUARIOS", "USUARIOS_ACTIVOS", "PARTIDAS_TOTAL", "PARTIDAS_HOY", "GANANCIA_CASA_TOTAL", "GANANCIA_CASA_HOY", "DEPOSITOS_TOTAL", "DEPOSITOS_HOY", "PROMEDIO_APUESTA", "PROMEDIO_APUESTA_HOY", "TOTAL_APOSTADO_HOY", "JUEGO_MAS_JUGADO", "PARTIDAS_JUEGO_MAS_JUGADO", "USUARIO_MAS_ACTIVO", "PARTIDAS_USUARIO_MAS_ACTIVO", "USUARIOS_ACTIVOS_HOY") AS 
  WITH
    u_na AS (SELECT * FROM usuarios WHERE id_rol != 1),
    p_na AS (
        SELECT p.* FROM partidas p
         WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = p.id_usuario AND u.id_rol = 1)
    ),
    t_na AS (
        SELECT t.* FROM transacciones t
         WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = t.id_usuario AND u.id_rol = 1)
    ),
    ea AS (SELECT id_estado FROM estado_usuario WHERE nombre = 'activo'),
    hoy AS (SELECT TRUNC(SYSDATE) AS d FROM DUAL)
SELECT
    (SELECT COUNT(*) FROM u_na)                                                                              AS total_usuarios,
    (SELECT COUNT(*) FROM u_na WHERE id_estado = (SELECT id_estado FROM ea))                                 AS usuarios_activos,
    (SELECT COUNT(*) FROM p_na)                                                                              AS partidas_total,
    (SELECT COUNT(*) FROM p_na WHERE fecha >= (SELECT d FROM hoy) AND fecha < (SELECT d + 1 FROM hoy))        AS partidas_hoy,
    (SELECT NVL(SUM(apuesta - ganancia), 0) FROM p_na)                                                       AS ganancia_casa_total,
    (SELECT NVL(SUM(apuesta - ganancia), 0) FROM p_na
      WHERE fecha >= (SELECT d FROM hoy) AND fecha < (SELECT d + 1 FROM hoy))                                 AS ganancia_casa_hoy,
    (SELECT NVL(SUM(monto), 0) FROM t_na WHERE tipo = 'deposito')                                            AS depositos_total,
    (SELECT NVL(SUM(monto), 0) FROM t_na
      WHERE tipo = 'deposito' AND fecha >= (SELECT d FROM hoy) AND fecha < (SELECT d + 1 FROM hoy))           AS depositos_hoy,
    (SELECT NVL(AVG(apuesta), 0) FROM p_na)                                                                  AS promedio_apuesta,
    (SELECT NVL(AVG(apuesta), 0) FROM p_na
      WHERE fecha >= (SELECT d FROM hoy) AND fecha < (SELECT d + 1 FROM hoy))                                 AS promedio_apuesta_hoy,
    (SELECT NVL(SUM(apuesta), 0) FROM p_na
      WHERE fecha >= (SELECT d FROM hoy) AND fecha < (SELECT d + 1 FROM hoy))                                 AS total_apostado_hoy,
    (SELECT nombre FROM (
        SELECT j.nombre, COUNT(*) AS cnt
        FROM p_na p JOIN juegos j ON p.id_juego = j.id_juego
        GROUP BY j.id_juego, j.nombre
        ORDER BY cnt DESC
    ) WHERE ROWNUM = 1)                                                                                      AS juego_mas_jugado,
    (SELECT cnt FROM (
        SELECT COUNT(*) AS cnt
        FROM p_na
        GROUP BY id_juego
        ORDER BY cnt DESC
    ) WHERE ROWNUM = 1)                                                                                      AS partidas_juego_mas_jugado,
    (SELECT nombre FROM (
        SELECT u.nombre_1 || ' ' || u.apellido_1 AS nombre, COUNT(*) AS cnt
        FROM p_na p JOIN usuarios u ON p.id_usuario = u.id_usuario
        GROUP BY u.id_usuario, u.nombre_1, u.apellido_1
        ORDER BY cnt DESC
    ) WHERE ROWNUM = 1)                                                                                      AS usuario_mas_activo,
    (SELECT cnt FROM (
        SELECT COUNT(*) AS cnt
        FROM p_na
        GROUP BY id_usuario
        ORDER BY cnt DESC
    ) WHERE ROWNUM = 1)                                                                                      AS partidas_usuario_mas_activo,
    (SELECT COUNT(DISTINCT id_usuario) FROM (
        SELECT id_usuario FROM p_na
         WHERE fecha >= (SELECT d FROM hoy) AND fecha < (SELECT d + 1 FROM hoy)
         UNION
        SELECT id_usuario FROM t_na
         WHERE fecha >= (SELECT d FROM hoy) AND fecha < (SELECT d + 1 FROM hoy)
    ))                                                                                                        AS usuarios_activos_hoy
FROM DUAL
;
