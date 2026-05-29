using System.Collections.ObjectModel;
using System.Windows;
using BLL;
using ENTITY;
using GUI.Commands;
using GUI.Views;

namespace GUI.ViewModels;

public class GestionUsuariosViewModel : ViewModelBase
{
    private readonly UsuarioServicio _usuarioSvc = new();
    public ObservableCollection<Usuario> Usuarios { get; } = new();

    public RelayCommand EditarUsuarioCommand { get; }
    public RelayCommand RefrescarCommand { get; }

    public GestionUsuariosViewModel()
    {
        EditarUsuarioCommand = new RelayCommand(EditarUsuario);
        RefrescarCommand = new RelayCommand(_ => CargarUsuarios());
        CargarUsuarios();
    }

    private void CargarUsuarios()
    {
        Usuarios.Clear();
        try
        {
            var lista = _usuarioSvc.ObtenerTodos();
            foreach (var u in lista)
                Usuarios.Add(u);
        }
        catch { }
    }

    private void EditarUsuario(object? parameter)
    {
        if (parameter is not Usuario usuario) return;

        var vm = new EditarUsuarioViewModel(usuario);
        var window = new EditarUsuarioWindow(vm);

        vm.DialogClosed += (s, e) => window.Close();

        window.Owner = Application.Current.Windows.Count > 0
            ? Application.Current.Windows[0] : null;
        window.ShowDialog();

        CargarUsuarios();
    }
}
