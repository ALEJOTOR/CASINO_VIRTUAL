--------------------------------------------------------
--  DDL for Procedure PR_APLICAR_BONO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "SEBAS_CASINO"."PR_APLICAR_BONO" (
    p_id_usuario    IN  NUMBER,
    p_id_bono       IN  NUMBER,
    p_monto         IN  NUMBER,
    p_descripcion   IN  VARCHAR2 DEFAULT NULL,
    p_resultado     OUT VARCHAR2
) AS
    v_ya_tiene  NUMBER := 0;
BEGIN
    SELECT COUNT(*) INTO v_ya_tiene
    FROM usuario_bonos
    WHERE id_usuario = p_id_usuario
      AND id_bono    = p_id_bono
      AND TRUNC(fecha_aplicado) = TRUNC(SYSDATE);

    IF v_ya_tiene > 0 THEN
        p_resultado := 'El bono ya fue aplicado hoy.';
        RETURN;
    END IF;

    INSERT INTO usuario_bonos (
        id_usuario_bono, id_usuario, id_bono, monto_aplicado,
        fecha_aplicado, estado, descripcion
    ) VALUES (
        seq_usuario_bonos.NEXTVAL, p_id_usuario, p_id_bono, p_monto,
        SYSTIMESTAMP, 'aplicado', p_descripcion
    );

    -- El trigger trg_actualizar_saldo actualiza el saldo (+)
    INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
    VALUES (seq_transacciones.NEXTVAL, p_id_usuario, 'bono', p_monto,
            SYSTIMESTAMP, NVL(p_descripcion, 'Bono aplicado'));

    COMMIT;
    p_resultado := 'Guardado correctamente.';
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        p_resultado := 'Error al aplicar bono: ' || SQLERRM;
END PR_APLICAR_BONO;

/
