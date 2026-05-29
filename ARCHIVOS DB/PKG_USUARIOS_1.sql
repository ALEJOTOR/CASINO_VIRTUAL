--------------------------------------------------------
--  DDL for Package Body PKG_USUARIOS
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "SEBAS_CASINO"."PKG_USUARIOS" AS
 
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


    -- ──────────────────────────────────────────────────────────
    -- FUNCIÓN: fn_calcular_ganancia_neta
    --
    -- Ganancia neta = lo que el usuario recibió en premios
    --                 menos lo que apostó en total.
    -- Valor positivo → el usuario le ganó al casino.
    -- Valor negativo → el casino le ganó al usuario.
    -- ──────────────────────────────────────────────────────────
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


    -- ──────────────────────────────────────────────────────────
    -- FUNCIÓN: fn_usuario_puede_apostar
    --
    -- Segunda línea de defensa además de la validación en BLL.
    -- Consulta el estado del usuario mediante la vista
    -- vw_usuarios_detalle para no repetir el JOIN.
    -- Retorna 1 (puede) o 0 (no puede).
    -- ──────────────────────────────────────────────────────────
    FUNCTION fn_usuario_puede_apostar(
        p_id_usuario IN NUMBER,
        p_monto      IN NUMBER
    ) RETURN NUMBER IS
        v_saldo  NUMBER(12,2);
        v_estado VARCHAR2(30);
    BEGIN
        -- Usamos la vista ya creada para evitar repetir el JOIN
        SELECT saldo, estado
          INTO v_saldo, v_estado
          FROM vw_usuarios_detalle
         WHERE id_usuario = p_id_usuario;

        -- El usuario debe estar activo Y tener saldo suficiente
        IF v_estado = 'activo' AND v_saldo >= p_monto THEN
            RETURN 1;
        ELSE
            RETURN 0;
        END IF;

    EXCEPTION
        -- Si el usuario no existe, definitivamente no puede apostar
        WHEN NO_DATA_FOUND THEN
            RETURN 0;
    END fn_usuario_puede_apostar;


    -- ──────────────────────────────────────────────────────────
    -- PROCEDIMIENTO: pr_realizar_deposito
    --
    --
    -- Solo inserta la transacción de tipo 'deposito'.
    -- El trigger trg_actualizar_saldo actualiza el saldo
    -- en usuarios automáticamente tras el INSERT.
    --
    -- Validaciones propias (además del CHECK de la tabla):
    --   - El monto debe ser mayor a 0
    --   - El usuario debe existir
    -- ──────────────────────────────────────────────────────────
    PROCEDURE pr_realizar_deposito(
        p_id_usuario IN  NUMBER,
        p_monto      IN  NUMBER,
        p_resultado  OUT VARCHAR2
    ) IS
        v_existe NUMBER;
    BEGIN
        -- Validar monto positivo
        IF p_monto <= 0 THEN
            p_resultado := 'El monto debe ser mayor a 0.';
            RETURN;
        END IF;

        -- Validar que el usuario existe
        SELECT COUNT(*)
          INTO v_existe
          FROM usuarios
         WHERE id_usuario = p_id_usuario;

        IF v_existe = 0 THEN
            p_resultado := 'Usuario no encontrado.';
            RETURN;
        END IF;

        -- Insertar la transacción. El trigger se encarga del saldo.
        INSERT INTO transacciones (
            id_transaccion, id_usuario, tipo,
            monto, fecha, descripcion
        ) VALUES (
            seq_transacciones.NEXTVAL, p_id_usuario, 'deposito',
            p_monto, CURRENT_TIMESTAMP, 'Recarga de saldo'
        );

        COMMIT;
        p_resultado := 'Deposito realizado correctamente.';

    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            p_resultado := 'Error al realizar deposito: ' || SQLERRM;
    END pr_realizar_deposito;


    -- ──────────────────────────────────────────────────────────
    -- PROCEDIMIENTO: pr_registrar_usuario
    --
    --
    -- Encapsula la búsqueda del id_estado por nombre ('activo')
    -- y el uso de la secuencia seq_usuarios.
    --
    -- ──────────────────────────────────────────────────────────
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

        SELECT id_estado
          INTO v_id_estado
          FROM estado_usuario
         WHERE nombre = 'activo';

        INSERT INTO usuarios (
            id_usuario, username, password,
            nombre_1, nombre_2,
            apellido_1, apellido_2,
            correo, fecha_nacimiento,
            saldo, id_rol, id_estado, fecha_registro
        ) VALUES (
            p_id_usuario, p_username, p_password,
            p_nombre_1, p_nombre_2,
            p_apellido_1, p_apellido_2,
            p_correo, p_fecha_nac,
            0, p_id_rol, v_id_estado, SYSDATE
        );

        COMMIT;
        p_resultado := 'Guardado correctamente.';

    EXCEPTION
        -- Violación de UNIQUE (username o correo duplicado)
        WHEN DUP_VAL_ON_INDEX THEN
            ROLLBACK;
            p_resultado := 'El username o correo ya están registrados.';
        WHEN OTHERS THEN
            ROLLBACK;
            p_resultado := 'Error al registrar usuario: ' || SQLERRM;
    END pr_registrar_usuario;

END PKG_USUARIOS;

/
