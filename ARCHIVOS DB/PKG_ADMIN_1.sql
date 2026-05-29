--------------------------------------------------------
--  DDL for Package Body PKG_ADMIN
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "SEBAS_CASINO"."PKG_ADMIN" AS

    FUNCTION fn_resumen_general RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR SELECT * FROM vw_resumen_admin;
        RETURN cur;
    END fn_resumen_general;

    FUNCTION fn_top_jugadores(p_cantidad NUMBER) RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR SELECT * FROM (
            SELECT * FROM vw_top_jugadores
        ) WHERE ROWNUM <= p_cantidad;
        RETURN cur;
    END fn_top_jugadores;

    FUNCTION fn_top_depositantes(p_cantidad NUMBER) RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR SELECT * FROM (
            SELECT * FROM vw_top_depositantes
        ) WHERE ROWNUM <= p_cantidad;
        RETURN cur;
    END fn_top_depositantes;

    FUNCTION fn_rentabilidad_por_juego RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR SELECT * FROM vw_rentabilidad_juegos;
        RETURN cur;
    END fn_rentabilidad_por_juego;

    FUNCTION fn_partidas_recientes(p_cantidad NUMBER) RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR
            SELECT p.*
            FROM partidas p
            WHERE NOT EXISTS (
                SELECT 1 FROM usuarios u
                WHERE u.id_usuario = p.id_usuario AND u.id_rol = 1
            )
            ORDER BY p.fecha DESC;
        RETURN cur;
    END fn_partidas_recientes;

    FUNCTION fn_movimientos_por_mes(p_meses NUMBER) RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR
            WITH t_na AS (
                SELECT t.* FROM transacciones t
                WHERE NOT EXISTS (
                    SELECT 1 FROM usuarios u
                    WHERE u.id_usuario = t.id_usuario AND u.id_rol = 1
                )
            ),
            meses AS (
                SELECT ADD_MONTHS(TRUNC(SYSDATE, 'MM'), -(LEVEL - 1)) AS mes
                FROM DUAL CONNECT BY LEVEL <= p_meses
            )
            SELECT
                TO_CHAR(m.mes, 'MON YY') AS mes,
                NVL((SELECT SUM(monto) FROM t_na WHERE tipo = 'deposito' AND TRUNC(fecha, 'MM') = m.mes), 0) AS depositos,
                NVL((SELECT SUM(monto) FROM t_na WHERE tipo = 'retiro' AND TRUNC(fecha, 'MM') = m.mes), 0)   AS retiros
            FROM meses m
            ORDER BY m.mes;
        RETURN cur;
    END fn_movimientos_por_mes;

    FUNCTION fn_ingresos_por_dia(p_dias NUMBER) RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR
            WITH p_na AS (
                SELECT p.* FROM partidas p
                WHERE NOT EXISTS (
                    SELECT 1 FROM usuarios u
                    WHERE u.id_usuario = p.id_usuario AND u.id_rol = 1
                )
            ),
            t_na AS (
                SELECT t.* FROM transacciones t
                WHERE NOT EXISTS (
                    SELECT 1 FROM usuarios u
                    WHERE u.id_usuario = t.id_usuario AND u.id_rol = 1
                )
            ),
            dias AS (
                SELECT TRUNC(SYSDATE) - LEVEL + 1 AS fecha
                FROM DUAL CONNECT BY LEVEL <= p_dias
            )
            SELECT
                d.fecha,
                NVL((SELECT SUM(monto) FROM t_na WHERE tipo = 'deposito' AND TRUNC(fecha) = d.fecha), 0)       AS depositos,
                NVL((SELECT SUM(apuesta - ganancia) FROM p_na WHERE TRUNC(fecha) = d.fecha), 0)                AS ganancia_casa
            FROM dias d
            ORDER BY d.fecha;
        RETURN cur;
    END fn_ingresos_por_dia;

END PKG_ADMIN;

/
