--------------------------------------------------------
--  DDL for Function FN_OBTENER_APUESTAS_PARTIDA
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE FUNCTION "SEBAS_CASINO"."FN_OBTENER_APUESTAS_PARTIDA" (
    p_id_partida IN NUMBER
) RETURN SYS_REFCURSOR AS
    cur SYS_REFCURSOR;
BEGIN
    OPEN cur FOR
        SELECT id_apuesta, id_partida, tipo_apuesta, numero_apuesta,
               monto, multiplicador, ganancia, resultado
        FROM apuestas
        WHERE id_partida = p_id_partida
        ORDER BY id_apuesta;
    RETURN cur;
END fn_obtener_apuestas_partida;

/
