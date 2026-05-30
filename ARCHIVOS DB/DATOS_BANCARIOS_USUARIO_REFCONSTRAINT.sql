--------------------------------------------------------
--  Ref Constraints for Table DATOS_BANCARIOS_USUARIO
--------------------------------------------------------

  ALTER TABLE "SEBAS_CASINO"."DATOS_BANCARIOS_USUARIO" ADD CONSTRAINT "FK_DB_USR" FOREIGN KEY ("ID_USUARIO")
	  REFERENCES "SEBAS_CASINO"."USUARIOS" ("ID_USUARIO") ENABLE;
