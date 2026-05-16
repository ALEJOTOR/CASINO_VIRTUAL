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
            new InicializadorBLL().InicializarTodo();
            Application.Run(new FrmLogin());
        }
    }
}
