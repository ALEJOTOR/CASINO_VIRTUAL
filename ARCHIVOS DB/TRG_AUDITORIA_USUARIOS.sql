--------------------------------------------------------
--  DDL for Trigger TRG_AUDITORIA_USUARIOS
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "SEBAS_CASINO"."TRG_AUDITORIA_USUARIOS" 
AFTER UPDATE OF saldo, id_estado, id_rol ON usuarios
FOR EACH ROW
BEGIN
    -- Auditar cambio de saldo
    IF :OLD.saldo != :NEW.saldo THEN
        INSERT INTO auditoria (
            id_auditoria, tabla_afectada, id_registro,
            operacion, campo, valor_anterior, valor_nuevo
        ) VALUES (
            seq_auditoria.NEXTVAL, 'usuarios', :OLD.id_usuario,
            'UPDATE', 'saldo',
            TO_CHAR(:OLD.saldo), TO_CHAR(:NEW.saldo)
        );
    END IF;

    -- Auditar cambio de estado
    IF :OLD.id_estado != :NEW.id_estado THEN
        INSERT INTO auditoria (
            id_auditoria, tabla_afectada, id_registro,
            operacion, campo, valor_anterior, valor_nuevo
        ) VALUES (
            seq_auditoria.NEXTVAL, 'usuarios', :OLD.id_usuario,
            'UPDATE', 'id_estado',
            TO_CHAR(:OLD.id_estado), TO_CHAR(:NEW.id_estado)
        );
    END IF;

    -- Auditar cambio de rol
    IF :OLD.id_rol != :NEW.id_rol THEN
        INSERT INTO auditoria (
            id_auditoria, tabla_afectada, id_registro,
            operacion, campo, valor_anterior, valor_nuevo
        ) VALUES (
            seq_auditoria.NEXTVAL, 'usuarios', :OLD.id_usuario,
            'UPDATE', 'id_rol',
            TO_CHAR(:OLD.id_rol), TO_CHAR(:NEW.id_rol)
        );
    END IF;
END;
/
ALTER TRIGGER "SEBAS_CASINO"."TRG_AUDITORIA_USUARIOS" ENABLE;
