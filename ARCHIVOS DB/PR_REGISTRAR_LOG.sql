--------------------------------------------------------
--  DDL for Procedure PR_REGISTRAR_LOG
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_REGISTRAR_LOG" (
    p_tipo_evento  IN VARCHAR2,
    p_nivel        IN VARCHAR2,
    p_id_usuario   IN NUMBER   DEFAULT NULL,
    p_ip_origen    IN VARCHAR2 DEFAULT NULL,
    p_modulo       IN VARCHAR2,
    p_descripcion  IN VARCHAR2
) AS
    PRAGMA AUTONOMOUS_TRANSACTION;
BEGIN
    INSERT INTO log_eventos (
        id_log, tipo_evento, nivel, id_usuario, ip_origen, modulo, descripcion, fecha
    ) VALUES (
        seq_log_eventos.NEXTVAL, p_tipo_evento, p_nivel, p_id_usuario,
        p_ip_origen, p_modulo, p_descripcion, SYSTIMESTAMP
    );
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        NULL;
END pr_registrar_log;

/
