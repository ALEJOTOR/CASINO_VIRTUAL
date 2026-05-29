--------------------------------------------------------
--  DDL for Function FN_CALCULAR_CASHBACK
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE FUNCTION "SEBAS_CASINO"."FN_CALCULAR_CASHBACK" (
    p_id_usuario IN NUMBER
) RETURN NUMBER AS
    v_perdidas    NUMBER := 0;
    v_ganancias   NUMBER := 0;
    v_porcentaje  NUMBER := 0;
BEGIN
    SELECT NVL(SUM(apuesta), 0), NVL(SUM(ganancia), 0)
    INTO v_perdidas, v_ganancias
    FROM partidas
    WHERE id_usuario = p_id_usuario
      AND fecha >= TRUNC(SYSDATE) - 7
      AND id_estado = 3;

    SELECT NVL(valor, 0) INTO v_porcentaje
    FROM bonos WHERE tipo = 'cashback' AND activo = 1 AND ROWNUM = 1;

    IF v_perdidas > v_ganancias THEN
        RETURN ROUND((v_perdidas - v_ganancias) * (v_porcentaje / 100), 2);
    END IF;
    RETURN 0;
END fn_calcular_cashback;

/
