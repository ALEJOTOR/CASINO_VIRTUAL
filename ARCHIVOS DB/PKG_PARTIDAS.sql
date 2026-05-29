--------------------------------------------------------
--  DDL for Package PKG_PARTIDAS
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "SEBAS_CASINO"."PKG_PARTIDAS" AS

    PROCEDURE pr_registrar_partida(
        p_id_usuario IN  NUMBER,
        p_id_juego   IN  NUMBER,
        p_id_estado  IN  NUMBER,
        p_apuesta    IN  NUMBER,
        p_ganancia   IN  NUMBER,
        p_msg        OUT VARCHAR2
    );

END PKG_PARTIDAS;

/
