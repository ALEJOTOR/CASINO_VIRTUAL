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

            try
            {
                new InicializadorServicio().InicializarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo inicializar la base de datos.\n\n" +
                    "Verifica la conexion Oracle, Hamachi y el archivo conexion.local.config.\n\n" +
                    ex.Message,
                    "Error de conexion",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            Application.Run(new FrmLogin());
        }
    }
}
