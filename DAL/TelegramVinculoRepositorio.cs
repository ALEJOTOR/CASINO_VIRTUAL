using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class TelegramVinculoRepositorio : OracleBase<TelegramVinculo>
    {
        public override IList<TelegramVinculo> Consultar()
        {
            IList<TelegramVinculo> lista = new List<TelegramVinculo>();
            using (OracleDataReader reader = EjecutarConsulta(
                @"SELECT id_vinculo, chat_id_telegram, id_usuario, username_casino,
                         codigo, estado, fecha_creacion, fecha_expiracion,
                         fecha_confirmacion, fecha_cancelacion
                    FROM telegram_vinculos
                   ORDER BY fecha_creacion DESC"))
            {
                while (reader.Read())
                    lista.Add(Mapear(reader));
            }
            return lista;
        }

        public override string Guardar(TelegramVinculo entidad)
        {
            EjecutarComando(
                @"INSERT INTO telegram_vinculos (
                      id_vinculo, chat_id_telegram, id_usuario, username_casino,
                      codigo, estado, fecha_creacion, fecha_expiracion
                  ) VALUES (
                      seq_telegram_vinculos.NEXTVAL, :chat_id, :id_usuario, :username,
                      :codigo, 'PENDIENTE', SYSTIMESTAMP, :fecha_expiracion
                  )",
                new[]
                {
                    (":chat_id", (object)entidad.ChatIdTelegram),
                    (":id_usuario", (object)entidad.IdUsuario),
                    (":username", (object)entidad.UsernameCasino),
                    (":codigo", (object)entidad.Codigo),
                    (":fecha_expiracion", (object)entidad.FechaExpiracion)
                });

            return "Guardado correctamente.";
        }

        public TelegramVinculo ObtenerPendientePorUsuario(int idUsuario)
        {
            using (OracleDataReader reader = EjecutarConsulta(
                @"SELECT id_vinculo, chat_id_telegram, id_usuario, username_casino,
                         codigo, estado, fecha_creacion, fecha_expiracion,
                         fecha_confirmacion, fecha_cancelacion
                    FROM telegram_vinculos
                   WHERE id_usuario = :id_usuario
                     AND estado = 'PENDIENTE'
                     AND fecha_expiracion > SYSTIMESTAMP
                   ORDER BY fecha_creacion DESC
                   FETCH FIRST 1 ROWS ONLY",
                new[] { (":id_usuario", (object)idUsuario) }))
            {
                if (reader.Read())
                    return Mapear(reader);
                return null;
            }
        }

        public string ConfirmarVinculo(int idUsuario, string codigo)
        {
            EjecutarComando(
                @"UPDATE telegram_vinculos
                     SET estado = 'VINCULADO',
                         fecha_confirmacion = SYSTIMESTAMP
                   WHERE id_usuario = :id_usuario
                     AND codigo = :codigo
                     AND estado = 'PENDIENTE'
                     AND fecha_expiracion > SYSTIMESTAMP",
                new[]
                {
                    (":id_usuario", (object)idUsuario),
                    (":codigo", (object)codigo)
                });

            return "Guardado correctamente.";
        }

        public bool ExisteVinculoActivo(int idUsuario)
        {
            int total = EjecutarScalar<int>(
                @"SELECT COUNT(*)
                    FROM telegram_vinculos
                   WHERE id_usuario = :id_usuario
                     AND estado = 'VINCULADO'",
                new[] { (":id_usuario", (object)idUsuario) });

            return total > 0;
        }

        private TelegramVinculo Mapear(OracleDataReader r)
        {
            return new TelegramVinculo
            {
                IdVinculo = r.GetInt32(0),
                ChatIdTelegram = r.GetString(1),
                IdUsuario = r.GetInt32(2),
                UsernameCasino = r.GetString(3),
                Codigo = r.GetString(4),
                Estado = r.GetString(5),
                FechaCreacion = r.GetDateTime(6),
                FechaExpiracion = r.GetDateTime(7),
                FechaConfirmacion = r.IsDBNull(8) ? (DateTime?)null : r.GetDateTime(8),
                FechaCancelacion = r.IsDBNull(9) ? (DateTime?)null : r.GetDateTime(9)
            };
        }
    }
}
