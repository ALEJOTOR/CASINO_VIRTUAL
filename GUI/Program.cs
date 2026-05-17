using System;
using System.Windows.Forms;
using BLL;

namespace GUI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Crea los archivos de datos iniciales si no existen
            new InicializadorServicio().InicializarTodo();

            Application.Run(new FrmLogin());
        }
    }
}
