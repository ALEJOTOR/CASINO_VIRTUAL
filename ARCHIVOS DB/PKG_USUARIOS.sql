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

    -- Bonos
    PROCEDURE pr_aplicar_bono(
        p_id_usuario  IN  NUMBER,
        p_id_bono     IN  NUMBER,
        p_monto       IN  NUMBER,
        p_descripcion IN  VARCHAR2 DEFAULT NULL,
        p_resultado   OUT VARCHAR2
    );

    PROCEDURE pr_cashback_semanal(
        p_resultado   OUT VARCHAR2
    );

    -- Depositos
    PROCEDURE pr_solicitar_deposito(
        p_id_usuario   IN  NUMBER,
        p_monto        IN  NUMBER,
        p_id_metodo    IN  NUMBER DEFAULT NULL,
        p_descripcion  IN  VARCHAR2 DEFAULT NULL,
        p_id_deposito  OUT NUMBER,
        p_resultado    OUT VARCHAR2
    );

    PROCEDURE pr_confirmar_deposito(
        p_id_deposito       IN  NUMBER,
        p_referencia_wompi  IN  VARCHAR2 DEFAULT NULL,
        p_resultado         OUT VARCHAR2
    );

    PROCEDURE pr_rechazar_deposito(
        p_id_deposito   IN  NUMBER,
        p_motivo        IN  VARCHAR2 DEFAULT NULL,
        p_resultado     OUT VARCHAR2
    );

    -- Retiros
    PROCEDURE pr_solicitar_retiro(
        p_id_usuario  IN  NUMBER,
        p_monto       IN  NUMBER,
        p_id_metodo   IN  NUMBER DEFAULT NULL,
        p_id_retiro   OUT NUMBER,
        p_resultado   OUT VARCHAR2
    );

    PROCEDURE pr_aprobar_retiro(
        p_id_retiro        IN  NUMBER,
        p_id_admin         IN  NUMBER,
        p_referencia_wompi IN  VARCHAR2 DEFAULT NULL,
        p_resultado        OUT VARCHAR2
    );

    PROCEDURE pr_rechazar_retiro(
        p_id_retiro   IN  NUMBER,
        p_motivo      IN  VARCHAR2 DEFAULT NULL,
        p_resultado   OUT VARCHAR2
    );

    -- Wompi: Datos bancarios
    PROCEDURE pr_guardar_datos_bancarios(
        p_id_usuario     IN  NUMBER,
        p_banco_id       IN  VARCHAR2,
        p_banco_nombre   IN  VARCHAR2,
        p_tipo_cuenta    IN  VARCHAR2,
        p_numero_cuenta  IN  VARCHAR2,
        p_tipo_doc       IN  VARCHAR2,
        p_numero_doc     IN  VARCHAR2,
        p_nombre_titular IN  VARCHAR2,
        p_resultado      OUT VARCHAR2
    );

END PKG_USUARIOS;

/
