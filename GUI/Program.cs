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

            // Inicializar archivos de datos si no existen
            new InicializadorBLL().InicializarTodo();

            Application.Run(new FrmLogin());
        }
    }
}
