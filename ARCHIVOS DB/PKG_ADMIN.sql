--------------------------------------------------------
--  DDL for Package PKG_ADMIN
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "SEBAS_CASINO"."PKG_ADMIN" AS

    FUNCTION fn_resumen_general RETURN SYS_REFCURSOR;

    FUNCTION fn_top_jugadores(p_cantidad NUMBER) RETURN SYS_REFCURSOR;

    FUNCTION fn_top_depositantes(p_cantidad NUMBER) RETURN SYS_REFCURSOR;

    FUNCTION fn_rentabilidad_por_juego RETURN SYS_REFCURSOR;

    FUNCTION fn_partidas_recientes(p_cantidad NUMBER) RETURN SYS_REFCURSOR;

    FUNCTION fn_movimientos_por_mes(p_meses NUMBER) RETURN SYS_REFCURSOR;

    FUNCTION fn_ingresos_por_dia(p_dias NUMBER) RETURN SYS_REFCURSOR;

END PKG_ADMIN;

/
