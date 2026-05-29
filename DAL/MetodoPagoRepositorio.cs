using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace DAL
{
    public class MetodoPagoRepositorio : OracleBase<MetodoPago>
    {
        public override IList<MetodoPago> Consultar()
        {
            IList<MetodoPago> lista = new List<MetodoPago>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_metodo, tipo, descripcion, activo FROM metodos_pago WHERE activo = 1 ORDER BY id_metodo"))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public override string Guardar(MetodoPago entity)
        {
            throw new System.NotSupportedException();
        }

        private MetodoPago Mapear(OracleDataReader r)
        {
            return new MetodoPago
            {
                IdMetodo = r.GetInt32(0),
                Tipo = r.GetString(1),
                Descripcion = r.IsDBNull(2) ? null : r.GetString(2),
                Activo = r.GetInt32(3) == 1
            };
        }
    }
}
