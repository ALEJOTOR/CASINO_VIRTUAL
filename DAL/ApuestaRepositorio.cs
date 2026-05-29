using ENTITY;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class ApuestaRepositorio : OracleBase<Apuesta>
    {
        public override IList<Apuesta> Consultar()
        {
            IList<Apuesta> lista = new List<Apuesta>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_apuesta, id_partida, tipo_apuesta, numero_apuesta, monto, multiplicador, ganancia, resultado FROM vw_detalle_apuestas ORDER BY id_apuesta DESC"))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public IList<Apuesta> ObtenerPorPartida(int idPartida)
        {
            IList<Apuesta> lista = new List<Apuesta>();
            string sql = @"SELECT id_apuesta, id_partida, tipo_apuesta, numero_apuesta,
                                  monto, multiplicador, ganancia, resultado
                             FROM apuestas
                            WHERE id_partida = :id
                            ORDER BY id_apuesta";

            using (OracleDataReader reader = EjecutarConsulta(sql, new[] { (":id", (object)idPartida) }))
                while (reader.Read())
                    lista.Add(new Apuesta
                    {
                        IdApuesta = reader.GetInt32(0),
                        IdPartida = reader.GetInt32(1),
                        TipoApuesta = reader.GetString(2),
                        NumeroApuesta = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                        Monto = reader.GetDecimal(4),
                        Multiplicador = reader.GetDecimal(5),
                        Ganancia = reader.GetDecimal(6),
                        Resultado = reader.GetString(7)
                    });

            return lista;
        }

        public override string Guardar(Apuesta a)
        {
            return EjecutarSP("pr_registrar_apuesta", "p_resultado_op",
                ("p_id_partida", a.IdPartida),
                ("p_tipo_apuesta", a.TipoApuesta),
                ("p_numero", (object)a.NumeroApuesta ?? DBNull.Value),
                ("p_monto", a.Monto),
                ("p_multiplicador", a.Multiplicador),
                ("p_ganancia", a.Ganancia),
                ("p_resultado", a.Resultado ?? "perdida"));
        }

        private Apuesta Mapear(OracleDataReader reader)
        {
            return new Apuesta
            {
                IdApuesta = reader.GetInt32(0),
                IdPartida = reader.GetInt32(1),
                TipoApuesta = reader.GetString(2),
                NumeroApuesta = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                Monto = reader.GetDecimal(4),
                Multiplicador = reader.GetDecimal(5),
                Ganancia = reader.GetDecimal(6),
                Resultado = reader.GetString(7)
            };
        }
    }
}
