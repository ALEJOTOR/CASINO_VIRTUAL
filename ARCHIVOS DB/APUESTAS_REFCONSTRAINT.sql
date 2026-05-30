--------------------------------------------------------
--  Ref Constraints for Table APUESTAS
--------------------------------------------------------

  ALTER TABLE "SEBAS_CASINO"."APUESTAS" ADD CONSTRAINT "FK_APU_PAR" FOREIGN KEY ("ID_PARTIDA")
	  REFERENCES "SEBAS_CASINO"."PARTIDAS" ("ID_PARTIDA") ENABLE;
