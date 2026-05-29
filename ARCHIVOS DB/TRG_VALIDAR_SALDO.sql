--------------------------------------------------------
--  DDL for Trigger TRG_VALIDAR_SALDO
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "SEBAS_CASINO"."TRG_VALIDAR_SALDO" 
BEFORE INSERT ON transacciones
FOR EACH ROW
DECLARE
    v_saldo NUMBER(12,2);
BEGIN
    IF :NEW.tipo IN ('perdida', 'retiro') THEN
        SELECT saldo INTO v_saldo
          FROM usuarios
         WHERE id_usuario = :NEW.id_usuario;

        IF v_saldo < :NEW.monto THEN
            RAISE_APPLICATION_ERROR(
                -20001,
                'Saldo insuficiente. Saldo actual: ' || TO_CHAR(v_saldo)
            );
        END IF;
    END IF;
END;
/
ALTER TRIGGER "SEBAS_CASINO"."TRG_VALIDAR_SALDO" ENABLE;
