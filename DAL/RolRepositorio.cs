using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.IO;

namespace DAL
{
    /// <summary>
    /// Solo necesita Consultar() y Guardar().
    /// Los roles no se editan ni eliminan desde la aplicación,
    /// se gestionan directamente desde el script SQL inicial.
    /// </summary>
    public class RolRepositorio : OracleBase<Rol>
    {
        public override IList<Rol> Consultar()
        {
            IList<Rol> lista = new List<Rol>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_rol, nombre_rol, descripcion FROM roles ORDER BY id_rol"))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public override string Guardar(Rol r)
        {
            return EjecutarComando(
                "INSERT INTO roles VALUES (seq_roles.NEXTVAL, :nombre, :descripcion)",
                new[]
                {
                    (":nombre",      (object)r.NombreRol),
                    (":descripcion", (object)(r.Descripcion ?? (object)System.DBNull.Value))
                });
        }

        private Rol Mapear(OracleDataReader r)
        {
            return new Rol
            {
                IdRol = r.GetInt32(0),
                NombreRol = r.GetString(1),
                Descripcion = r.IsDBNull(2) ? null : r.GetString(2)
            };
        }
    }
}
