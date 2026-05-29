--------------------------------------------------------
--  Ref Constraints for Table TRANSACCIONES
--------------------------------------------------------

  ALTER TABLE "SEBAS_CASINO"."TRANSACCIONES" ADD CONSTRAINT "FK_TRANSACCIONES_USUARIO" FOREIGN KEY ("ID_USUARIO")
	  REFERENCES "SEBAS_CASINO"."USUARIOS" ("ID_USUARIO") ENABLE;
