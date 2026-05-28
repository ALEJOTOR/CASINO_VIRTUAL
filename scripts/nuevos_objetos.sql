-- ============================================================
-- Script: nuevos_objetos.sql
-- Descripci�n: Nuevos objetos BD para el Casino Virtual
--   - Procedimientos pr_actualizar_usuario, pr_cambiar_estado
--   - Vista vw_resumen_admin (extendida con 5 columnas)
--   - Vista vw_rentabilidad_juegos
--   - Vista vw_ingresos_diarios
--   - Vista vw_top_jugadores
--   - Vista vw_top_depositantes
--   - Vista vw_movimientos_mensuales
--   - Package PKG_ADMIN (8 funciones ref cursor)
--   - Tabla + trigger trg_auditoria_usuarios
-- ============================================================

-- ============================================================
-- 1. pr_actualizar_usuario
--    Reemplaza el SQL crudo en UsuarioRepositorio.Actualizar()
-- ============================================================
CREATE OR REPLACE PROCEDURE pr_actualizar_usuario(
    p_id_usuario     IN  NUMBER,
    p_username       IN  VARCHAR2,
    p_password       IN  VARCHAR2,
    p_nombre_1       IN  VARCHAR2,
    p_nombre_2       IN  VARCHAR2 DEFAULT NULL,
    p_apellido_1     IN  VARCHAR2,
    p_apellido_2     IN  VARCHAR2 DEFAULT NULL,
    p_correo         IN  VARCHAR2,
    p_fecha_nac      IN  DATE,
    p_id_rol         IN  NUMBER,
    p_estado         IN  VARCHAR2,
    p_resultado      OUT VARCHAR2
) AS
BEGIN
    UPDATE usuarios SET
        username         = p_username,
        password         = p_password,
        nombre_1         = p_nombre_1,
        nombre_2         = p_nombre_2,
        apellido_1       = p_apellido_1,
        apellido_2       = p_apellido_2,
        correo           = p_correo,
        fecha_nacimiento = p_fecha_nac,
        id_rol           = p_id_rol,
        id_estado        = (SELECT id_estado FROM estado_usuario WHERE nombre = p_estado)
    WHERE id_usuario = p_id_usuario;

    IF SQL%ROWCOUNT = 0 THEN
        p_resultado := 'Usuario no encontrado.';
    ELSE
        COMMIT;
        p_resultado := 'Guardado correctamente.';
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        p_resultado := 'Error al actualizar usuario: ' || SQLERRM;
END pr_actualizar_usuario;
/

-- ============================================================
-- 2. pr_cambiar_estado
--    Cambia el estado de un usuario sin recargar todos sus datos
-- ============================================================
CREATE OR REPLACE PROCEDURE pr_cambiar_estado(
    p_id_usuario     IN  NUMBER,
    p_nuevo_estado   IN  VARCHAR2,
    p_resultado      OUT VARCHAR2
) AS
BEGIN
    UPDATE usuarios SET
        id_estado = (SELECT id_estado FROM estado_usuario WHERE nombre = p_nuevo_estado)
    WHERE id_usuario = p_id_usuario;

    IF SQL%ROWCOUNT = 0 THEN
        p_resultado := 'Usuario no encontrado.';
    ELSE
        COMMIT;
        p_resultado := 'Guardado correctamente.';
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        p_resultado := 'Error al cambiar estado: ' || SQLERRM;
END pr_cambiar_estado;
/

-- ============================================================
-- 3. vw_resumen_admin
--    M�tricas agregadas del casino en una sola fila (16 columnas).
--    Reemplaza AdminServicio.ObtenerResumenGeneral().
-- ============================================================
CREATE OR REPLACE VIEW vw_resumen_admin AS
WITH
    u_na AS (SELECT * FROM usuarios WHERE id_rol != 1),
    p_na AS (
        SELECT p.* FROM partidas p
         WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = p.id_usuario AND u.id_rol = 1)
    ),
    t_na AS (
        SELECT t.* FROM transacciones t
         WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = t.id_usuario AND u.id_rol = 1)
    ),
    ea AS (SELECT id_estado FROM estado_usuario WHERE nombre = 'activo'),
    hoy AS (SELECT TRUNC(SYSDATE) AS d FROM DUAL)
SELECT
    (SELECT COUNT(*) FROM u_na)                                                                              AS total_usuarios,
    (SELECT COUNT(*) FROM u_na WHERE id_estado = (SELECT id_estado FROM ea))                                 AS usuarios_activos,
    (SELECT COUNT(*) FROM p_na)                                                                              AS partidas_total,
    (SELECT COUNT(*) FROM p_na WHERE fecha >= (SELECT d FROM hoy) AND fecha < (SELECT d + 1 FROM hoy))        AS partidas_hoy,
    (SELECT NVL(SUM(apuesta - ganancia), 0) FROM p_na)                                                       AS ganancia_casa_total,
    (SELECT NVL(SUM(apuesta - ganancia), 0) FROM p_na
      WHERE fecha >= (SELECT d FROM hoy) AND fecha < (SELECT d + 1 FROM hoy))                                 AS ganancia_casa_hoy,
    (SELECT NVL(SUM(monto), 0) FROM t_na WHERE tipo = 'deposito')                                            AS depositos_total,
    (SELECT NVL(SUM(monto), 0) FROM t_na
      WHERE tipo = 'deposito' AND fecha >= (SELECT d FROM hoy) AND fecha < (SELECT d + 1 FROM hoy))           AS depositos_hoy,
    (SELECT NVL(AVG(apuesta), 0) FROM p_na)                                                                  AS promedio_apuesta,
    (SELECT NVL(AVG(apuesta), 0) FROM p_na
      WHERE fecha >= (SELECT d FROM hoy) AND fecha < (SELECT d + 1 FROM hoy))                                 AS promedio_apuesta_hoy,
    (SELECT NVL(SUM(apuesta), 0) FROM p_na
      WHERE fecha >= (SELECT d FROM hoy) AND fecha < (SELECT d + 1 FROM hoy))                                 AS total_apostado_hoy,
    (SELECT nombre FROM (
        SELECT j.nombre, COUNT(*) AS cnt
        FROM p_na p JOIN juegos j ON p.id_juego = j.id_juego
        GROUP BY j.id_juego, j.nombre
        ORDER BY cnt DESC
    ) WHERE ROWNUM = 1)                                                                                      AS juego_mas_jugado,
    (SELECT cnt FROM (
        SELECT COUNT(*) AS cnt
        FROM p_na
        GROUP BY id_juego
        ORDER BY cnt DESC
    ) WHERE ROWNUM = 1)                                                                                      AS partidas_juego_mas_jugado,
    (SELECT nombre FROM (
        SELECT u.nombre_1 || ' ' || u.apellido_1 AS nombre, COUNT(*) AS cnt
        FROM p_na p JOIN usuarios u ON p.id_usuario = u.id_usuario
        GROUP BY u.id_usuario, u.nombre_1, u.apellido_1
        ORDER BY cnt DESC
    ) WHERE ROWNUM = 1)                                                                                      AS usuario_mas_activo,
    (SELECT cnt FROM (
        SELECT COUNT(*) AS cnt
        FROM p_na
        GROUP BY id_usuario
        ORDER BY cnt DESC
    ) WHERE ROWNUM = 1)                                                                                      AS partidas_usuario_mas_activo,
    (SELECT COUNT(*) FROM u_na WHERE id_estado = (SELECT id_estado FROM ea))                                 AS usuarios_activos_hoy
FROM DUAL;
/

-- ============================================================
-- 4. vw_rentabilidad_juegos
--    Rentabilidad por juego (partidas, apostado, ganancia,
--    margen %). Reemplaza AdminServicio.ObtenerRentabilidadPorJuego()
-- ============================================================
CREATE OR REPLACE VIEW vw_rentabilidad_juegos AS
WITH p_na AS (
    SELECT p.* FROM partidas p
     WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = p.id_usuario AND u.id_rol = 1)
)
SELECT
    j.nombre                                   AS juego,
    COUNT(p.id_partida)                         AS total_partidas,
    NVL(SUM(p.apuesta), 0)                      AS total_apostado,
    NVL(SUM(p.apuesta - p.ganancia), 0)         AS ganancia_casa,
    ROUND(
        CASE WHEN NVL(SUM(p.apuesta), 0) > 0
             THEN (SUM(p.apuesta - p.ganancia) / SUM(p.apuesta)) * 100
             ELSE 0
        END, 2
    )                                           AS margen_porcentaje
FROM juegos j
LEFT JOIN p_na p ON j.id_juego = p.id_juego
GROUP BY j.id_juego, j.nombre
ORDER BY ganancia_casa DESC;
/

-- ============================================================
-- 5. vw_ingresos_diarios
--    Ingresos (dep�sitos) y ganancia casa agregados por d�a
--    de los �ltimos 30 d�as.
-- ============================================================
CREATE OR REPLACE VIEW vw_ingresos_diarios AS
WITH p_na AS (
    SELECT p.* FROM partidas p
     WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = p.id_usuario AND u.id_rol = 1)
),
t_na AS (
    SELECT t.* FROM transacciones t
     WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = t.id_usuario AND u.id_rol = 1)
),
dias AS (
    SELECT TRUNC(SYSDATE) - LEVEL + 1 AS fecha FROM DUAL CONNECT BY LEVEL <= 30
)
SELECT
    d.fecha,
    NVL((SELECT SUM(monto) FROM t_na WHERE tipo = 'deposito' AND TRUNC(fecha) = d.fecha), 0) AS depositos,
    NVL((SELECT SUM(apuesta - ganancia) FROM p_na WHERE TRUNC(fecha) = d.fecha), 0)           AS ganancia_casa
FROM dias d
ORDER BY d.fecha;
/

-- ============================================================
-- 6. vw_top_jugadores
--    Top N jugadores por total apostado (excluye admins).
-- ============================================================
CREATE OR REPLACE VIEW vw_top_jugadores AS
SELECT
    u.id_usuario,
    u.username,
    u.nombre_1 || ' ' || u.apellido_1              AS nombre_completo,
    u.saldo,
    COUNT(p.id_partida)                              AS total_partidas,
    SUM(CASE WHEN p.id_estado = 2 THEN 1 ELSE 0 END) AS partidas_ganadas,
    SUM(CASE WHEN p.id_estado = 3 THEN 1 ELSE 0 END) AS partidas_perdidas,
    NVL(SUM(p.apuesta), 0)                           AS total_apostado,
    NVL(SUM(p.ganancia), 0)                          AS total_ganado,
    NVL(SUM(p.ganancia), 0) - NVL(SUM(p.apuesta), 0) AS ganancia_neta
FROM usuarios u
JOIN partidas p ON u.id_usuario = p.id_usuario
WHERE u.id_rol != 1
GROUP BY u.id_usuario, u.username, u.nombre_1, u.apellido_1, u.saldo
ORDER BY total_apostado DESC;
/

-- ============================================================
-- 7. vw_top_depositantes
--    Ranking de usuarios por total depositado (excluye admins).
-- ============================================================
CREATE OR REPLACE VIEW vw_top_depositantes AS
SELECT
    u.id_usuario,
    u.nombre_1 || ' ' || u.apellido_1   AS nombre_completo,
    u.username,
    NVL(SUM(t.monto), 0)                 AS total_depositado,
    COUNT(t.id_transaccion)              AS num_depositos
FROM usuarios u
JOIN transacciones t ON u.id_usuario = t.id_usuario
WHERE u.id_rol != 1 AND t.tipo = 'deposito'
GROUP BY u.id_usuario, u.nombre_1, u.apellido_1, u.username
ORDER BY total_depositado DESC;
/

-- ============================================================
-- 8. vw_movimientos_mensuales
--    Dep�sitos y retiros agregados por mes (�ltimos 12).
-- ============================================================
CREATE OR REPLACE VIEW vw_movimientos_mensuales AS
WITH t_na AS (
    SELECT t.* FROM transacciones t
     WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = t.id_usuario AND u.id_rol = 1)
),
meses AS (
    SELECT ADD_MONTHS(TRUNC(SYSDATE, 'MM'), -(LEVEL - 1)) AS mes
    FROM DUAL CONNECT BY LEVEL <= 12
)
SELECT
    TO_CHAR(m.mes, 'MON YY') AS mes,
    NVL((SELECT SUM(monto) FROM t_na WHERE tipo = 'deposito' AND TRUNC(fecha, 'MM') = m.mes), 0) AS depositos,
    NVL((SELECT SUM(monto) FROM t_na WHERE tipo = 'retiro' AND TRUNC(fecha, 'MM') = m.mes), 0)   AS retiros
FROM meses m
ORDER BY m.mes;
/

-- ============================================================
-- 9. PKG_ADMIN
--    Package con funciones que encapsulan consultas de
--    administraci�n usando las vistas vw_*.  Cada funci�n
--    devuelve un SYS_REFCURSOR.
-- ============================================================
CREATE OR REPLACE PACKAGE PKG_ADMIN AS

    FUNCTION fn_resumen_general RETURN SYS_REFCURSOR;

    FUNCTION fn_top_jugadores(p_cantidad NUMBER) RETURN SYS_REFCURSOR;

    FUNCTION fn_top_depositantes(p_cantidad NUMBER) RETURN SYS_REFCURSOR;

    FUNCTION fn_rentabilidad_por_juego RETURN SYS_REFCURSOR;

    FUNCTION fn_partidas_recientes(p_cantidad NUMBER) RETURN SYS_REFCURSOR;

    FUNCTION fn_movimientos_por_mes(p_meses NUMBER) RETURN SYS_REFCURSOR;

    FUNCTION fn_ingresos_por_dia(p_dias NUMBER) RETURN SYS_REFCURSOR;

END PKG_ADMIN;
/

CREATE OR REPLACE PACKAGE BODY PKG_ADMIN AS

    FUNCTION fn_resumen_general RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR SELECT * FROM vw_resumen_admin;
        RETURN cur;
    END fn_resumen_general;

    FUNCTION fn_top_jugadores(p_cantidad NUMBER) RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR SELECT * FROM (
            SELECT * FROM vw_top_jugadores
        ) WHERE ROWNUM <= p_cantidad;
        RETURN cur;
    END fn_top_jugadores;

    FUNCTION fn_top_depositantes(p_cantidad NUMBER) RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR SELECT * FROM (
            SELECT * FROM vw_top_depositantes
        ) WHERE ROWNUM <= p_cantidad;
        RETURN cur;
    END fn_top_depositantes;

    FUNCTION fn_rentabilidad_por_juego RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR SELECT * FROM vw_rentabilidad_juegos;
        RETURN cur;
    END fn_rentabilidad_por_juego;

    FUNCTION fn_partidas_recientes(p_cantidad NUMBER) RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR
            SELECT p.*
            FROM partidas p
            WHERE NOT EXISTS (
                SELECT 1 FROM usuarios u
                WHERE u.id_usuario = p.id_usuario AND u.id_rol = 1
            )
            ORDER BY p.fecha DESC;
        RETURN cur;
    END fn_partidas_recientes;

    FUNCTION fn_movimientos_por_mes(p_meses NUMBER) RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR
            WITH t_na AS (
                SELECT t.* FROM transacciones t
                WHERE NOT EXISTS (
                    SELECT 1 FROM usuarios u
                    WHERE u.id_usuario = t.id_usuario AND u.id_rol = 1
                )
            ),
            meses AS (
                SELECT ADD_MONTHS(TRUNC(SYSDATE, 'MM'), -(LEVEL - 1)) AS mes
                FROM DUAL CONNECT BY LEVEL <= p_meses
            )
            SELECT
                TO_CHAR(m.mes, 'MON YY') AS mes,
                NVL((SELECT SUM(monto) FROM t_na WHERE tipo = 'deposito' AND TRUNC(fecha, 'MM') = m.mes), 0) AS depositos,
                NVL((SELECT SUM(monto) FROM t_na WHERE tipo = 'retiro' AND TRUNC(fecha, 'MM') = m.mes), 0)   AS retiros
            FROM meses m
            ORDER BY m.mes;
        RETURN cur;
    END fn_movimientos_por_mes;

    FUNCTION fn_ingresos_por_dia(p_dias NUMBER) RETURN SYS_REFCURSOR IS
        cur SYS_REFCURSOR;
    BEGIN
        OPEN cur FOR
            WITH p_na AS (
                SELECT p.* FROM partidas p
                WHERE NOT EXISTS (
                    SELECT 1 FROM usuarios u
                    WHERE u.id_usuario = p.id_usuario AND u.id_rol = 1
                )
            ),
            t_na AS (
                SELECT t.* FROM transacciones t
                WHERE NOT EXISTS (
                    SELECT 1 FROM usuarios u
                    WHERE u.id_usuario = t.id_usuario AND u.id_rol = 1
                )
            ),
            dias AS (
                SELECT TRUNC(SYSDATE) - LEVEL + 1 AS fecha
                FROM DUAL CONNECT BY LEVEL <= p_dias
            )
            SELECT
                d.fecha,
                NVL((SELECT SUM(monto) FROM t_na WHERE tipo = 'deposito' AND TRUNC(fecha) = d.fecha), 0)       AS depositos,
                NVL((SELECT SUM(apuesta - ganancia) FROM p_na WHERE TRUNC(fecha) = d.fecha), 0)                AS ganancia_casa
            FROM dias d
            ORDER BY d.fecha;
        RETURN cur;
    END fn_ingresos_por_dia;

END PKG_ADMIN;
/

-- ============================================================
-- 10. trg_auditoria_usuarios
--    Registra cambios en saldo e id_estado de usuarios.
-- ============================================================

-- Tabla de auditor�a
CREATE TABLE auditoria_usuarios (
    id_auditoria   NUMBER        PRIMARY KEY,
    id_usuario     NUMBER        NOT NULL,
    campo          VARCHAR2(50)  NOT NULL,
    valor_anterior VARCHAR2(4000),
    valor_nuevo    VARCHAR2(4000),
    fecha          TIMESTAMP     DEFAULT SYSTIMESTAMP,
    usuario_bd     VARCHAR2(100),
    CONSTRAINT fk_audit_usr FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
);

-- Secuencia para la PK
CREATE SEQUENCE seq_auditoria_usuarios;

-- Trigger que captura cambios en saldo e id_estado
CREATE OR REPLACE TRIGGER trg_auditoria_usuarios
    AFTER UPDATE OF saldo, id_estado ON usuarios
    FOR EACH ROW
DECLARE
    v_usuario VARCHAR2(100);
BEGIN
    SELECT USER INTO v_usuario FROM DUAL;

    IF :OLD.saldo IS NULL OR :NEW.saldo IS NULL OR :OLD.saldo != :NEW.saldo THEN
        INSERT INTO auditoria_usuarios (
            id_auditoria, id_usuario, campo,
            valor_anterior, valor_nuevo, usuario_bd
        ) VALUES (
            seq_auditoria_usuarios.NEXTVAL, :OLD.id_usuario, 'saldo',
            TO_CHAR(:OLD.saldo), TO_CHAR(:NEW.saldo), v_usuario
        );
    END IF;

    IF :OLD.id_estado IS NULL OR :NEW.id_estado IS NULL OR :OLD.id_estado != :NEW.id_estado THEN
        INSERT INTO auditoria_usuarios (
            id_auditoria, id_usuario, campo,
            valor_anterior, valor_nuevo, usuario_bd
        ) VALUES (
            seq_auditoria_usuarios.NEXTVAL, :OLD.id_usuario, 'id_estado',
            TO_CHAR(:OLD.id_estado), TO_CHAR(:NEW.id_estado), v_usuario
        );
    END IF;
END trg_auditoria_usuarios;
/

PROMPT 'Ejecuci�n completada. Nuevos objetos creados exitosamente.'
