using System.Windows;

namespace GUI;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        try
        {
            new BLL.InicializadorServicio().InicializarTodo();
        }
        catch (System.Exception ex)
        {
            string detalles = ex.Message;
            Exception? inner = ex.InnerException;
            while (inner != null)
            {
                detalles += $"\n  → {inner.GetType().Name}: {inner.Message}";
                inner = inner.InnerException;
            }
            MessageBox.Show(
                "No se pudo inicializar la base de datos.\n\n" +
                "Verifica la conexión Oracle, Hamachi y el archivo conexion.local.config.\n\n" + detalles,
                "Error de conexión",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown();
            return;
        }
        new Views.LoginWindow().Show();
    }
}
