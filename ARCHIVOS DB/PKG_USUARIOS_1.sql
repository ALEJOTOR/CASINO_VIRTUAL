--------------------------------------------------------
--  DDL for Package Body PKG_USUARIOS
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "SEBAS_CASINO"."PKG_USUARIOS" AS

    -- ===== FUNCIONES =====

    FUNCTION fn_total_depositado(
        p_id_usuario IN NUMBER
    ) RETURN NUMBER IS
        v_total NUMBER(12,2);
    BEGIN
        SELECT NVL(SUM(monto), 0)
          INTO v_total
          FROM transacciones
         WHERE id_usuario = p_id_usuario
           AND tipo       = 'deposito';
        RETURN v_total;
    END fn_total_depositado;

    FUNCTION fn_calcular_ganancia_neta(
        p_id_usuario IN NUMBER
    ) RETURN NUMBER IS
        v_total_ganado   NUMBER(12,2);
        v_total_apostado NUMBER(12,2);
    BEGIN
        SELECT NVL(SUM(ganancia), 0),
               NVL(SUM(apuesta),  0)
          INTO v_total_ganado,
               v_total_apostado
          FROM partidas
         WHERE id_usuario = p_id_usuario;
        RETURN v_total_ganado - v_total_apostado;
    END fn_calcular_ganancia_neta;

    FUNCTION fn_usuario_puede_apostar(
        p_id_usuario IN NUMBER,
        p_monto      IN NUMBER
    ) RETURN NUMBER IS
        v_saldo  NUMBER(12,2);
        v_estado VARCHAR2(30);
    BEGIN
        SELECT saldo, estado
          INTO v_saldo, v_estado
          FROM vw_usuarios_detalle
         WHERE id_usuario = p_id_usuario;
        IF v_estado = 'activo' AND v_saldo >= p_monto THEN
            RETURN 1;
        ELSE
            RETURN 0;
        END IF;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN RETURN 0;
    END fn_usuario_puede_apostar;

    -- ===== DEPOSITO BÁSICO (legacy, sin flujo Wompi) =====

    PROCEDURE pr_realizar_deposito(
        p_id_usuario IN  NUMBER,
        p_monto      IN  NUMBER,
        p_resultado  OUT VARCHAR2
    ) IS
        v_existe NUMBER;
    BEGIN
        IF p_monto <= 0 THEN
            p_resultado := 'El monto debe ser mayor a 0.';
            RETURN;
        END IF;
        SELECT COUNT(*) INTO v_existe FROM usuarios WHERE id_usuario = p_id_usuario;
        IF v_existe = 0 THEN
            p_resultado := 'Usuario no encontrado.';
            RETURN;
        END IF;
        -- TRG_ACTUALIZAR_SALDO actualiza saldo automáticamente. NO hacer UPDATE aquí.
        INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
        VALUES (seq_transacciones.NEXTVAL, p_id_usuario, 'deposito',
                p_monto, CURRENT_TIMESTAMP, 'Recarga de saldo');
        COMMIT;
        p_resultado := 'Deposito realizado correctamente.';
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error al realizar deposito: ' || SQLERRM;
    END pr_realizar_deposito;

    -- ===== REGISTRO DE USUARIO =====

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
    ) IS
        v_id_estado NUMBER;
    BEGIN
        SELECT id_estado INTO v_id_estado FROM estado_usuario WHERE nombre = 'activo';
        INSERT INTO usuarios (
            id_usuario, username, password,
            nombre_1, nombre_2, apellido_1, apellido_2,
            correo, fecha_nacimiento,
            saldo, id_rol, id_estado, fecha_registro
        ) VALUES (
            p_id_usuario, p_username, p_password,
            p_nombre_1, p_nombre_2, p_apellido_1, p_apellido_2,
            p_correo, p_fecha_nac,
            0, p_id_rol, v_id_estado, SYSDATE
        );
        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN DUP_VAL_ON_INDEX THEN ROLLBACK; p_resultado := 'El username o correo ya estan registrados.';
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error al registrar usuario: ' || SQLERRM;
    END pr_registrar_usuario;

    -- ===== BONOS =====

    PROCEDURE pr_aplicar_bono(
        p_id_usuario  IN  NUMBER,
        p_id_bono     IN  NUMBER,
        p_monto       IN  NUMBER,
        p_descripcion IN  VARCHAR2 DEFAULT NULL,
        p_resultado   OUT VARCHAR2
    ) AS
        v_ya_tiene NUMBER := 0;
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

        -- ✅ CORREGIDO: eliminado UPDATE usuarios SET saldo.
        -- TRG_ACTUALIZAR_SALDO maneja tipo 'bono' → saldo = saldo + monto.
        INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
        VALUES (seq_transacciones.NEXTVAL, p_id_usuario, 'bono', p_monto,
                SYSTIMESTAMP, NVL(p_descripcion, 'Bono aplicado'));

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error al aplicar bono: ' || SQLERRM;
    END pr_aplicar_bono;

    PROCEDURE pr_cashback_semanal(
        p_resultado OUT VARCHAR2
    ) AS
        v_id_bono    NUMBER;
        v_monto      NUMBER;
        v_procesados NUMBER := 0;
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
                                'Cashback semanal automatico', p_resultado);
                v_procesados := v_procesados + 1;
            END IF;
        END LOOP;
        COMMIT;
        p_resultado := 'Cashback procesado para ' || v_procesados || ' usuarios.';
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error en cashback semanal: ' || SQLERRM;
    END pr_cashback_semanal;

    -- ===== DEPOSITOS WOMPI =====

    PROCEDURE pr_solicitar_deposito(
        p_id_usuario   IN  NUMBER,
        p_monto        IN  NUMBER,
        p_id_metodo    IN  NUMBER DEFAULT NULL,
        p_descripcion  IN  VARCHAR2 DEFAULT NULL,
        p_id_deposito  OUT NUMBER,
        p_resultado    OUT VARCHAR2
    ) AS
    BEGIN
        INSERT INTO depositos (
            id_deposito, id_usuario, id_metodo, monto, estado,
            fecha_solicitud, descripcion
        ) VALUES (
            seq_depositos.NEXTVAL, p_id_usuario, p_id_metodo, p_monto,
            'pendiente', SYSTIMESTAMP, p_descripcion
        ) RETURNING id_deposito INTO p_id_deposito;

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            p_id_deposito := -1;
            p_resultado := 'Error al crear solicitud: ' || SQLERRM;
    END pr_solicitar_deposito;

    PROCEDURE pr_confirmar_deposito(
        p_id_deposito       IN  NUMBER,
        p_referencia_wompi  IN  VARCHAR2 DEFAULT NULL,
        p_resultado         OUT VARCHAR2
    ) AS
        v_id_usuario NUMBER;
        v_monto      NUMBER;
        v_estado     VARCHAR2(20);
    BEGIN
        SELECT id_usuario, monto, estado
          INTO v_id_usuario, v_monto, v_estado
          FROM depositos
         WHERE id_deposito = p_id_deposito
        FOR UPDATE;

        IF v_estado NOT IN ('pendiente', 'procesando') THEN
            p_resultado := 'El deposito no esta en estado procesable.';
            RETURN;
        END IF;

        UPDATE depositos SET
            estado              = 'aprobado',
            fecha_procesamiento = SYSTIMESTAMP,
            referencia_wompi    = p_referencia_wompi
        WHERE id_deposito = p_id_deposito;

        -- ✅ CORREGIDO: eliminado UPDATE usuarios SET saldo.
        -- TRG_ACTUALIZAR_SALDO maneja tipo 'deposito' → saldo = saldo + monto.
        INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
        VALUES (seq_transacciones.NEXTVAL, v_id_usuario, 'deposito', v_monto,
                SYSTIMESTAMP, 'Deposito aprobado. Ref Wompi: ' || NVL(p_referencia_wompi, 'N/A'));

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN NO_DATA_FOUND THEN ROLLBACK; p_resultado := 'Deposito no encontrado.';
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error al confirmar deposito: ' || SQLERRM;
    END pr_confirmar_deposito;

    PROCEDURE pr_rechazar_deposito(
        p_id_deposito IN  NUMBER,
        p_motivo      IN  VARCHAR2 DEFAULT NULL,
        p_resultado   OUT VARCHAR2
    ) AS
    BEGIN
        UPDATE depositos SET
            estado              = 'rechazado',
            fecha_procesamiento = SYSTIMESTAMP,
            error_detalle       = p_motivo
        WHERE id_deposito = p_id_deposito
          AND estado IN ('pendiente', 'procesando');

        IF SQL%ROWCOUNT = 0 THEN
            p_resultado := 'Deposito no encontrado o no procesable.';
        ELSE
            COMMIT;
            p_resultado := 'Guardado correctamente.';
        END IF;
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error: ' || SQLERRM;
    END pr_rechazar_deposito;

    -- ===== RETIROS =====

    PROCEDURE pr_solicitar_retiro(
        p_id_usuario IN  NUMBER,
        p_monto      IN  NUMBER,
        p_id_metodo  IN  NUMBER DEFAULT NULL,
        p_id_retiro  OUT NUMBER,
        p_resultado  OUT VARCHAR2
    ) AS
        v_saldo NUMBER;
    BEGIN
        SELECT saldo INTO v_saldo
        FROM usuarios
        WHERE id_usuario = p_id_usuario FOR UPDATE;

        IF v_saldo < p_monto THEN
            p_resultado := 'Saldo insuficiente para el retiro.';
            RETURN;
        END IF;

        INSERT INTO retiros (
            id_retiro, id_usuario, id_metodo, monto, estado, fecha_solicitud
        ) VALUES (
            seq_retiros.NEXTVAL, p_id_usuario, p_id_metodo, p_monto,
            'pendiente', SYSTIMESTAMP
        ) RETURNING id_retiro INTO p_id_retiro;

        -- ✅ CORREGIDO: eliminado UPDATE usuarios SET saldo - p_monto.
        -- TRG_ACTUALIZAR_SALDO maneja tipo 'retiro' → saldo = saldo - monto.
        INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
        VALUES (seq_transacciones.NEXTVAL, p_id_usuario, 'retiro', p_monto,
                SYSTIMESTAMP, 'Retiro solicitado. Retiro ID: ' || p_id_retiro);

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            p_id_retiro := -1;
            p_resultado := 'Error al solicitar retiro: ' || SQLERRM;
    END pr_solicitar_retiro;

    PROCEDURE pr_aprobar_retiro(
        p_id_retiro        IN  NUMBER,
        p_id_admin         IN  NUMBER,
        p_referencia_wompi IN  VARCHAR2 DEFAULT NULL,
        p_resultado        OUT VARCHAR2
    ) AS
        v_id_usuario NUMBER;
        v_monto      NUMBER;
    BEGIN
        SELECT id_usuario, monto INTO v_id_usuario, v_monto
        FROM retiros WHERE id_retiro = p_id_retiro AND estado = 'pendiente'
        FOR UPDATE;

        UPDATE retiros SET
            estado              = 'aprobado',
            fecha_procesamiento = SYSTIMESTAMP,
            id_admin_revisor    = p_id_admin,
            referencia_wompi    = p_referencia_wompi
        WHERE id_retiro = p_id_retiro;

        INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
        VALUES (seq_transacciones.NEXTVAL, v_id_usuario, 'retiro', v_monto,
                SYSTIMESTAMP, 'Retiro aprobado. Ref: ' || NVL(p_referencia_wompi, 'N/A'));

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN NO_DATA_FOUND THEN ROLLBACK; p_resultado := 'Retiro no encontrado o ya procesado.';
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error: ' || SQLERRM;
    END pr_aprobar_retiro;

    PROCEDURE pr_rechazar_retiro(
        p_id_retiro  IN  NUMBER,
        p_motivo     IN  VARCHAR2 DEFAULT NULL,
        p_resultado  OUT VARCHAR2
    ) AS
        v_id_usuario NUMBER;
        v_monto      NUMBER;
    BEGIN
        SELECT id_usuario, monto INTO v_id_usuario, v_monto
        FROM retiros WHERE id_retiro = p_id_retiro AND estado = 'pendiente'
        FOR UPDATE;

        UPDATE retiros SET
            estado              = 'rechazado',
            fecha_procesamiento = SYSTIMESTAMP,
            motivo_rechazo      = p_motivo
        WHERE id_retiro = p_id_retiro;

        -- ✅ CORREGIDO: eliminado UPDATE usuarios SET saldo + v_monto.
        -- Devolvemos el dinero insertando una transacción 'deposito' que
        -- TRG_ACTUALIZAR_SALDO convierte en saldo = saldo + monto.
        INSERT INTO transacciones (id_transaccion, id_usuario, tipo, monto, fecha, descripcion)
        VALUES (seq_transacciones.NEXTVAL, v_id_usuario, 'deposito', v_monto,
                SYSTIMESTAMP, 'Retiro rechazado - saldo devuelto. ID: ' || p_id_retiro
                              || '. Motivo: ' || NVL(p_motivo, 'N/A'));

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN NO_DATA_FOUND THEN ROLLBACK; p_resultado := 'Retiro no encontrado.';
        WHEN OTHERS THEN ROLLBACK; p_resultado := 'Error: ' || SQLERRM;
    END pr_rechazar_retiro;

    -- ===== WOMPI: DATOS BANCARIOS =====

    PROCEDURE pr_guardar_datos_bancarios(
        p_id_usuario     IN  NUMBER,
        p_banco_id       IN  VARCHAR2,
        p_tipo_cuenta    IN  VARCHAR2,
        p_numero_cuenta  IN  VARCHAR2,
        p_tipo_doc       IN  VARCHAR2,
        p_numero_doc     IN  VARCHAR2,
        p_nombre_titular IN  VARCHAR2,
        p_resultado      OUT VARCHAR2
    ) AS
        v_existentes NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_existentes
        FROM datos_bancarios_usuario
        WHERE id_usuario = p_id_usuario AND activo = 1;

        IF v_existentes > 0 THEN
            UPDATE datos_bancarios_usuario SET activo = 0
            WHERE id_usuario = p_id_usuario AND activo = 1;
        END IF;

        INSERT INTO datos_bancarios_usuario (
            id_datos_bancarios, id_usuario, banco_id, tipo_cuenta,
            numero_cuenta, tipo_doc, numero_doc, nombre_titular, activo
        ) VALUES (
            seq_datos_bancarios.NEXTVAL, p_id_usuario, p_banco_id, p_tipo_cuenta,
            p_numero_cuenta, p_tipo_doc, p_numero_doc, p_nombre_titular, 1
        );

        COMMIT;
        p_resultado := 'Guardado correctamente.';
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            p_resultado := 'Error al guardar datos bancarios: ' || SQLERRM;
    END pr_guardar_datos_bancarios;

END PKG_USUARIOS;

/
