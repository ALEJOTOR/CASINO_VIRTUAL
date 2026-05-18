using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public abstract class OracleBase<T>
    {
        // ── Método que cada repositorio hijo DEBE implementar ────

        /// <summary>
        /// Trae todos los registros de la tabla correspondiente.
        /// Igual que antes: la BLL llama Consultar() sin saber
        /// si por debajo hay un .txt o una base de datos.
        /// </summary>
        public abstract IList<T> Consultar();

        /// <summary>
        /// Inserta un registro nuevo en la tabla.
        /// Cada repositorio implementa el INSERT con sus columnas.
        /// Devuelve "Guardado correctamente." para no romper
        /// los mensajes que ya muestra la GUI.
        /// </summary>
        public abstract string Guardar(T entidad);

        // ── Método utilitario disponible para todos los hijos ────

        /// <summary>
        /// Ejecuta un comando SQL que no devuelve filas:
        /// INSERT, UPDATE o DELETE.
        /// 
        /// Recibe el SQL con parámetros con nombre (ej: :id_usuario)
        /// y una lista de pares (nombre, valor) para esos parámetros.
        /// Usar parámetros en lugar de concatenar strings es
        /// obligatorio para evitar SQL Injection.
        /// 
        /// Ejemplo de uso desde un repositorio hijo:
        ///   EjecutarComando(
        ///       "UPDATE usuarios SET saldo = :saldo WHERE id_usuario = :id",
        ///       new[] { (":saldo", (object)150.00m), (":id", (object)3) }
        ///   );
        /// </summary>
        protected string EjecutarComando(string sql, (string nombre, object valor)[] parametros = null)
        {
            // using garantiza que la conexión se cierre siempre,
            // incluso si ocurre una excepción dentro del bloque.
            using (OracleConnection con = ConexionOracle.Abrir())
            using (OracleCommand cmd = new OracleCommand(sql, con))
            {
                if (parametros != null)
                    foreach (var (nombre, valor) in parametros)
                        cmd.Parameters.Add(new OracleParameter(nombre, valor));

                cmd.ExecuteNonQuery();
                return "Guardado correctamente.";
            }
        }

        /// <summary>
        /// Ejecuta un SELECT y entrega el OracleDataReader al
        /// repositorio hijo para que mapee las filas a objetos T.
        /// 
        /// IMPORTANTE: el llamador es responsable de cerrar
        /// tanto el reader como la conexión. Por eso se usa
        /// CommandBehavior.CloseConnection — cuando el reader
        /// se cierra, la conexión se cierra automáticamente.
        /// 
        /// Ejemplo de uso desde un repositorio hijo:
        ///   using (var reader = EjecutarConsulta("SELECT * FROM roles"))
        ///       while (reader.Read())
        ///           lista.Add(Mapear(reader));
        /// </summary>
        protected OracleDataReader EjecutarConsulta(string sql, (string nombre, object valor)[] parametros = null)
        {
            OracleConnection con = ConexionOracle.Abrir();
            OracleCommand cmd = new OracleCommand(sql, con);

            if (parametros != null)
                foreach (var (nombre, valor) in parametros)
                    cmd.Parameters.Add(new OracleParameter(nombre, valor));

            // CloseConnection: cuando el reader se cierra,
            // cierra la conexión también. Así el using del
            // repositorio hijo maneja todo correctamente.
            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }
    }
}
