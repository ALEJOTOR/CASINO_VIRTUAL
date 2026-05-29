--------------------------------------------------------
--  DDL for Procedure PR_REGISTRAR_APUESTA
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_REGISTRAR_APUESTA" (
    p_id_partida    IN  NUMBER,
    p_tipo_apuesta  IN  VARCHAR2,
    p_numero        IN  NUMBER DEFAULT NULL,
    p_monto         IN  NUMBER,
    p_multiplicador IN  NUMBER,
    p_ganancia      IN  NUMBER,
    p_resultado     IN  VARCHAR2,
    p_resultado_op  OUT VARCHAR2
) AS
BEGIN
    INSERT INTO apuestas (
        id_apuesta, id_partida, tipo_apuesta, numero_apuesta,
        monto, multiplicador, ganancia, resultado
    ) VALUES (
        seq_apuestas.NEXTVAL, p_id_partida, p_tipo_apuesta, p_numero,
        p_monto, p_multiplicador, p_ganancia, p_resultado
    );
    COMMIT;
    p_resultado_op := 'Guardado correctamente.';
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        p_resultado_op := 'Error al registrar apuesta: ' || SQLERRM;
END pr_registrar_apuesta;

/
