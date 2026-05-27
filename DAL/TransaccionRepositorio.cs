using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class TransaccionRepositorio : OracleBase<Transaccion>
    {

        public override IList<Transaccion> Consultar()
        {
            IList<Transaccion> lista = new List<Transaccion>();

            string sql = @"SELECT id_transaccion, id_usuario, tipo,
                                  monto, fecha, descripcion
                             FROM transacciones
                            ORDER BY fecha DESC";

            using (OracleDataReader reader = EjecutarConsulta(sql))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public IList<Transaccion> ObtenerPorUsuario(int idUsuario)
        {
            IList<Transaccion> lista = new List<Transaccion>();

            string sql = @"SELECT id_transaccion, id_usuario, tipo,
                                  monto, fecha, descripcion
                             FROM transacciones
                            WHERE id_usuario = :id
                            ORDER BY fecha DESC";

            using (OracleDataReader reader = EjecutarConsulta(sql,
                new[] { (":id", (object)idUsuario) }))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public override string Guardar(Transaccion t)
        {
            string sql = @"INSERT INTO transacciones (
                               id_transaccion, id_usuario, tipo,
                               monto, fecha, descripcion
                           ) VALUES (
                               seq_transacciones.NEXTVAL, :id_usuario, :tipo,
                               :monto, CURRENT_TIMESTAMP, :descripcion
                           )";

            return EjecutarComando(sql, new[]
            {
                (":id_usuario",  (object)t.IdUsuario),
                (":tipo",        (object)t.Tipo),
                (":monto",       (object)t.Monto),
                (":descripcion", (object)(t.Descripcion ?? (object)DBNull.Value))
            });
        }

        // ── DEPÓSITO — ahora delega en PKG_USUARIOS.pr_realizar_deposito ──────
        // Antes: abría OracleTransaction manualmente, armaba el INSERT,
        //        hacía commit/rollback en C#.
        // Ahora: Oracle maneja todo eso internamente. El trigger
        //        trg_actualizar_saldo sigue actualizando el saldo igual que antes.

        public string RegistrarDepositoConSaldo(int idUsuario, decimal monto)
        {
            using (OracleConnection con = ConexionOracle.Abrir())
            using (OracleCommand cmd = new OracleCommand("PKG_USUARIOS.pr_realizar_deposito", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;

                cmd.Parameters.Add(new OracleParameter("p_id_usuario", idUsuario));
                cmd.Parameters.Add(new OracleParameter("p_monto", monto));

                // Parámetro OUT: recibe 'Deposito realizado correctamente.' o el error
                var pResultado = new OracleParameter("p_resultado", OracleDbType.Varchar2, 200)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(pResultado);

                cmd.ExecuteNonQuery();

                return pResultado.Value.ToString();
            }
        }

        // ── Mapear — no cambia ────────────────────────────────────────────────

        private Transaccion Mapear(OracleDataReader r)
        {
            return new Transaccion
            {
                IdTransaccion = r.GetInt32(0),
                IdUsuario = r.GetInt32(1),
                Tipo = r.GetString(2),
                Monto = r.GetDecimal(3),
                Fecha = r.GetDateTime(4),
                Descripcion = r.IsDBNull(5) ? null : r.GetString(5)
            };
        }
    }
}