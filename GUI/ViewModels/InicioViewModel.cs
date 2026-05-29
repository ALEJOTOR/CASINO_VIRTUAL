using BLL;
using ENTITY;
using GUI.Commands;

namespace GUI.ViewModels;

public class InicioViewModel : ViewModelBase
{
    private readonly Usuario _usuario;
    private readonly MainViewModel _mainVM;
    private string _bienvenido = "";

    public string Bienvenido { get => _bienvenido; set => SetProperty(ref _bienvenido, value); }

    public RelayCommand JugarMinasCommand { get; }
    public RelayCommand JugarRuletaCommand { get; }
    public RelayCommand JugarTragamonedasCommand { get; }

    public event EventHandler? JugarMinasSolicitado;
    public event EventHandler? JugarRuletaSolicitado;
    public event EventHandler? JugarTragamonedasSolicitado;

    public InicioViewModel(Usuario usuario, MainViewModel mainVM)
    {
        _usuario = usuario;
        _mainVM = mainVM;
        Bienvenido = $"Bienvenido, {usuario.Nombre1} {usuario.Apellido1}";

        JugarMinasCommand = new RelayCommand(_ => JugarMinasSolicitado?.Invoke(this, EventArgs.Empty));
        JugarRuletaCommand = new RelayCommand(_ => JugarRuletaSolicitado?.Invoke(this, EventArgs.Empty));
        JugarTragamonedasCommand = new RelayCommand(_ => JugarTragamonedasSolicitado?.Invoke(this, EventArgs.Empty));
    }

    public void RecargarSaldo() => _mainVM.ActualizarSaldo();
}
