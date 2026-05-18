using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;

namespace DAL
{
    /// <summary>
    /// El repositorio más importante junto con UsuarioRepositorio.
    /// Cada INSERT aquí dispara el trigger trg_actualizar_saldo
    /// en Oracle, que actualiza el saldo del usuario automáticamente.
    /// Por eso ActualizarSaldo() en UsuarioServicio (BLL) ya no
    /// necesita llamar al repositorio de usuarios — basta con
    /// insertar la transacción aquí.
    /// 
    /// Agrega ObtenerPorUsuario() para que la BLL no tenga
    /// que traer todas las transacciones y filtrar con foreach.
    /// </summary>
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

        // Trae solo las transacciones de un usuario específico.
        // Antes la BLL hacía foreach sobre todas — con muchos
        // usuarios eso es costoso. Oracle filtra en el servidor.
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

        // El INSERT más crítico del sistema.
        // Al ejecutarse, el trigger trg_validar_saldo verifica
        // que haya saldo suficiente (para perdida/retiro), y el
        // trigger trg_actualizar_saldo ajusta el saldo del usuario.
        // Si el saldo no alcanza, Oracle lanza una excepción con
        // el mensaje definido en el trigger (RAISE_APPLICATION_ERROR)
        // y el INSERT no se completa — todo queda consistente.
        //
        // SiguienteIdTransaccion() de PartidaServicio desaparece:
        // seq_transacciones.NEXTVAL lo reemplaza.
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

        private Transaccion Mapear(OracleDataReader r)
        {
            return new Transaccion
            {
                IdTransaccion = r.GetInt32(0),
                IdUsuario = r.GetInt32(1),
                Tipo = r.GetString(2),
                Monto = r.GetDecimal(3),
                // Oracle devuelve TIMESTAMP como DateTime
                Fecha = r.GetDateTime(4),
                Descripcion = r.IsDBNull(5) ? null : r.GetString(5)
            };
        }
    }
}
