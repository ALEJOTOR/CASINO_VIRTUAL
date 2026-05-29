--------------------------------------------------------
--  DDL for Function FN_OBTENER_LOG_RECIENTE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE FUNCTION "SEBAS_CASINO"."FN_OBTENER_LOG_RECIENTE" (
    p_cantidad IN NUMBER DEFAULT 100
) RETURN SYS_REFCURSOR AS
    cur SYS_REFCURSOR;
BEGIN
    OPEN cur FOR
        SELECT l.id_log, l.tipo_evento, l.nivel, l.id_usuario,
               u.username, l.ip_origen, l.modulo, l.descripcion, l.fecha
        FROM log_eventos l
        LEFT JOIN usuarios u ON l.id_usuario = u.id_usuario
        WHERE ROWNUM <= p_cantidad
        ORDER BY l.fecha DESC;
    RETURN cur;
END fn_obtener_log_reciente;

/
