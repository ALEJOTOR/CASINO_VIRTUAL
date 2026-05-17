using System.Collections.Generic;
using System.IO;

namespace DAL
{
    public abstract class ArchivoBase<T>
    {
        protected string _nombreArchivo;

        protected ArchivoBase(string nombreArchivo)
        {
            CrearCarpeta(nombreArchivo);
            _nombreArchivo = nombreArchivo;
        }

        private void CrearCarpeta(string ruta)
        {
            string carpeta = Path.GetDirectoryName(ruta);
            if (!string.IsNullOrEmpty(carpeta) && !Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        public string Guardar(T entidad)
        {
            StreamWriter escritor = new StreamWriter(_nombreArchivo, true);
            escritor.WriteLine(entidad.ToString());
            escritor.Close();
            return "Guardado correctamente.";
        }

        public string GuardarTodos(IList<T> entidades)
        {
            StreamWriter escritor = new StreamWriter(_nombreArchivo, false);
            foreach (T entidad in entidades)
                escritor.WriteLine(entidad.ToString());
            escritor.Close();
            return "Guardado correctamente.";
        }

        public abstract IList<T> Consultar();
    }
}
