--------------------------------------------------------
--  DDL for Package Body PKG_PARTIDAS
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "SEBAS_CASINO"."PKG_PARTIDAS" AS

    PROCEDURE pr_registrar_partida(
        p_id_usuario IN  NUMBER,
        p_id_juego   IN  NUMBER,
        p_id_estado  IN  NUMBER,
        p_apuesta    IN  NUMBER,
        p_ganancia   IN  NUMBER,
        p_msg        OUT VARCHAR2
    ) IS
    BEGIN
        IF PKG_USUARIOS.fn_usuario_puede_apostar(p_id_usuario, p_apuesta) = 0 THEN
            p_msg := 'Saldo insuficiente o usuario inactivo.';
            RETURN;
        END IF;

        INSERT INTO transacciones (
            id_transaccion, id_usuario, tipo,
            monto, fecha, descripcion
        ) VALUES (
            seq_transacciones.NEXTVAL, p_id_usuario, 'perdida',
            p_apuesta, CURRENT_TIMESTAMP,
            'Apuesta partida juego ' || p_id_juego
        );

        IF p_id_estado = 2 AND p_ganancia > 0 THEN
            INSERT INTO transacciones (
                id_transaccion, id_usuario, tipo,
                monto, fecha, descripcion
            ) VALUES (
                seq_transacciones.NEXTVAL, p_id_usuario, 'ganancia',
                p_ganancia, CURRENT_TIMESTAMP,
                'Ganancia partida juego ' || p_id_juego
            );
        END IF;

        INSERT INTO partidas (
            id_partida, id_usuario, id_juego,
            id_estado, fecha, apuesta,
            ganancia
        ) VALUES (
            seq_partidas.NEXTVAL, p_id_usuario, p_id_juego,
            p_id_estado, CURRENT_TIMESTAMP, p_apuesta,
            NVL(p_ganancia, 0)
        );

        COMMIT;
        p_msg := 'Guardado correctamente.';

    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            p_msg := 'Error al registrar partida: ' || SQLERRM;
    END pr_registrar_partida;

END PKG_PARTIDAS;

/
