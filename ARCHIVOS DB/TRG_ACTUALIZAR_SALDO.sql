--------------------------------------------------------
--  DDL for Trigger TRG_ACTUALIZAR_SALDO
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "SEBAS_CASINO"."TRG_ACTUALIZAR_SALDO" 
AFTER INSERT ON transacciones
FOR EACH ROW
BEGIN
    IF :NEW.tipo IN ('deposito', 'ganancia') THEN
        UPDATE usuarios
           SET saldo = saldo + :NEW.monto
         WHERE id_usuario = :NEW.id_usuario;

    ELSIF :NEW.tipo IN ('perdida', 'retiro') THEN
        UPDATE usuarios
           SET saldo = saldo - :NEW.monto
         WHERE id_usuario = :NEW.id_usuario;
    END IF;
END;
/
ALTER TRIGGER "SEBAS_CASINO"."TRG_ACTUALIZAR_SALDO" ENABLE;
