--------------------------------------------------------
--  DDL for Package PKG_USUARIOS
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "SEBAS_CASINO"."PKG_USUARIOS" AS
 
    FUNCTION fn_total_depositado(
        p_id_usuario IN NUMBER
    ) RETURN NUMBER;


    FUNCTION fn_calcular_ganancia_neta(
        p_id_usuario IN NUMBER
    ) RETURN NUMBER;


    FUNCTION fn_usuario_puede_apostar(
        p_id_usuario IN NUMBER,
        p_monto      IN NUMBER
    ) RETURN NUMBER;


    PROCEDURE pr_realizar_deposito(
        p_id_usuario IN  NUMBER,
        p_monto      IN  NUMBER,
        p_resultado  OUT VARCHAR2
    );


    PROCEDURE pr_registrar_usuario(
        p_id_usuario     IN  NUMBER,
        p_username       IN  VARCHAR2,
        p_password       IN  VARCHAR2,
        p_nombre_1       IN  VARCHAR2,
        p_nombre_2       IN  VARCHAR2,
        p_apellido_1     IN  VARCHAR2,
        p_apellido_2     IN  VARCHAR2,
        p_correo         IN  VARCHAR2,
        p_fecha_nac      IN  DATE,
        p_id_rol         IN  NUMBER,
        p_resultado      OUT VARCHAR2
    );

END PKG_USUARIOS;

/
