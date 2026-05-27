using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace DAL
{
    public class EstadoUsuarioRepositorio : OracleBase<EstadoUsuario>
    {
        public override IList<EstadoUsuario> Consultar()
        {
            IList<EstadoUsuario> lista = new List<EstadoUsuario>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_estado, nombre FROM estado_usuario ORDER BY id_estado"))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public override string Guardar(EstadoUsuario entity)
        {
            throw new System.NotImplementedException();
        }

        private EstadoUsuario Mapear(OracleDataReader r)
        {
            return new EstadoUsuario
            {
                IdEstado = r.GetInt32(0),
                Nombre = r.GetString(1)
            };
        }
    }
}
