-- ============================================================
-- TELEGRAM_INTEGRACION.sql
-- Descripcion: Vinculacion segura entre Telegram y usuarios del casino
-- Nota: No requiere triggers. La vinculacion se controla desde la API/App.
-- ============================================================

-- ============================================================
-- 1. SEQ_TELEGRAM_VINCULOS
-- ============================================================
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count
    FROM user_sequences
    WHERE sequence_name = 'SEQ_TELEGRAM_VINCULOS';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE SEQUENCE seq_telegram_vinculos MINVALUE 1 START WITH 1 INCREMENT BY 1 CACHE 20';
    END IF;
END;
/

-- ============================================================
-- 2. TELEGRAM_VINCULOS
-- ============================================================
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count
    FROM user_tables
    WHERE table_name = 'TELEGRAM_VINCULOS';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE TABLE telegram_vinculos (
            id_vinculo          NUMBER        PRIMARY KEY,
            chat_id_telegram    VARCHAR2(50)  NOT NULL,
            id_usuario          NUMBER        NOT NULL,
            username_casino     VARCHAR2(50)  NOT NULL,
            codigo              VARCHAR2(10)  NOT NULL,
            estado              VARCHAR2(20)  DEFAULT ''PENDIENTE'' NOT NULL,
            fecha_creacion      TIMESTAMP     DEFAULT SYSTIMESTAMP NOT NULL,
            fecha_expiracion    TIMESTAMP     NOT NULL,
            fecha_confirmacion  TIMESTAMP,
            fecha_cancelacion   TIMESTAMP,
            CONSTRAINT fk_tel_usr FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
            CONSTRAINT ck_tel_estado CHECK (estado IN (''PENDIENTE'', ''VINCULADO'', ''EXPIRADO'', ''CANCELADO''))
        ) TABLESPACE CASINO_VIRTUAL';
    END IF;
END;
/

-- ============================================================
-- 3. INDICES
-- ============================================================
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count
    FROM user_indexes
    WHERE index_name = 'IDX_TEL_CHAT';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE INDEX idx_tel_chat ON telegram_vinculos(chat_id_telegram)';
    END IF;

    SELECT COUNT(*) INTO v_count
    FROM user_indexes
    WHERE index_name = 'IDX_TEL_USUARIO';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE INDEX idx_tel_usuario ON telegram_vinculos(id_usuario)';
    END IF;

    SELECT COUNT(*) INTO v_count
    FROM user_indexes
    WHERE index_name = 'IDX_TEL_CODIGO';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE INDEX idx_tel_codigo ON telegram_vinculos(codigo)';
    END IF;

    SELECT COUNT(*) INTO v_count
    FROM user_indexes
    WHERE index_name = 'IDX_TEL_ESTADO';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE INDEX idx_tel_estado ON telegram_vinculos(estado)';
    END IF;
END;
/

-- ============================================================
-- 4. VW_TELEGRAM_VINCULOS
-- ============================================================
CREATE OR REPLACE VIEW vw_telegram_vinculos AS
SELECT
    tv.id_vinculo,
    tv.chat_id_telegram,
    tv.id_usuario,
    u.username,
    u.nombre_1,
    u.apellido_1,
    tv.username_casino,
    tv.codigo,
    tv.estado,
    tv.fecha_creacion,
    tv.fecha_expiracion,
    tv.fecha_confirmacion,
    tv.fecha_cancelacion
FROM telegram_vinculos tv
JOIN usuarios u ON u.id_usuario = tv.id_usuario;

PROMPT 'TELEGRAM_INTEGRACION.sql ejecutado correctamente.'
