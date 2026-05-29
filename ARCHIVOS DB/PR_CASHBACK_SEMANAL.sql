--------------------------------------------------------
--  DDL for Procedure PR_CASHBACK_SEMANAL
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_CASHBACK_SEMANAL" AS
    v_resultado  VARCHAR2(500);
    v_id_bono    NUMBER;
    v_monto      NUMBER;
BEGIN
    SELECT id_bono INTO v_id_bono
    FROM bonos WHERE tipo = 'cashback' AND activo = 1 AND ROWNUM = 1;

    FOR usr IN (
        SELECT u.id_usuario
        FROM usuarios u
        JOIN estado_usuario eu ON u.id_estado = eu.id_estado
        WHERE eu.nombre = 'activo' AND u.id_rol = 2
    ) LOOP
        v_monto := fn_calcular_cashback(usr.id_usuario);
        IF v_monto > 0 THEN
            pr_aplicar_bono(usr.id_usuario, v_id_bono, v_monto,
                            'Cashback semanal automatico', v_resultado);
        END IF;
    END LOOP;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN ROLLBACK;
END pr_cashback_semanal;

/
