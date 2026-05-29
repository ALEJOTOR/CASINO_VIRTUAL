using System.Windows;
using BLL;
using ENTITY;
using GUI.Commands;
using GUI.Services;
using GUI.Views;

namespace GUI.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly Usuario _usuario;
    private readonly UsuarioServicio _usuarioSvc = new();
    private readonly LogServicio _logSvc = new();
    private readonly INavigationService _navigationService;
    private string _saldoTexto = "";
    private string _nombreUsuario = "";

    public string SaldoTexto { get => _saldoTexto; set => SetProperty(ref _saldoTexto, value); }
    public string NombreUsuario { get => _nombreUsuario; set => SetProperty(ref _nombreUsuario, value); }
    public ViewModelBase? CurrentViewModel => _navigationService.CurrentViewModel;

    public RelayCommand IrInicioCommand { get; }
    public RelayCommand IrHistorialCommand { get; }
    public RelayCommand IrBilleteraCommand { get; }
    public RelayCommand IrMisBonosCommand { get; }
    public RelayCommand IrSoporteCommand { get; }
    public RelayCommand CerrarSesionCommand { get; }

    public MainViewModel(Usuario usuario)
    {
        _usuario = usuario;
        NombreUsuario = $"{usuario.Nombre1} {usuario.Apellido1}";

        NavigationService? navSvc = null;
        navSvc = new NavigationService(tipo =>
        {
            if (tipo == typeof(InicioViewModel))
            {
                var vm = new InicioViewModel(usuario, this);
                vm.JugarMinasSolicitado += (_, _) => navSvc!.NavigateTo<MinasViewModel>();
                vm.JugarRuletaSolicitado += (_, _) => navSvc!.NavigateTo<RuletaViewModel>();
                vm.JugarTragamonedasSolicitado += (_, _) => navSvc!.NavigateTo<TragamonedasViewModel>();
                return vm;
            }
            if (tipo == typeof(HistorialViewModel)) return new HistorialViewModel(usuario);
            if (tipo == typeof(BilleteraViewModel)) return new BilleteraViewModel(usuario, this);
            if (tipo == typeof(MisBonosViewModel)) return new MisBonosViewModel(usuario);
            if (tipo == typeof(SoporteViewModel)) return new SoporteViewModel();
            if (tipo == typeof(PerfilViewModel)) return new PerfilViewModel(usuario);
            if (tipo == typeof(MinasViewModel))
            {
                var vm = new MinasViewModel(usuario);
                vm.SaldoActualizado += (_, _) => ActualizarSaldo();
                return vm;
            }
            if (tipo == typeof(RuletaViewModel))
            {
                var vm = new RuletaViewModel(usuario);
                vm.SaldoActualizado += (_, _) => ActualizarSaldo();
                return vm;
            }
            if (tipo == typeof(TragamonedasViewModel))
            {
                var vm = new TragamonedasViewModel(usuario);
                vm.SaldoActualizado += (_, _) => ActualizarSaldo();
                return vm;
            }
            return new InicioViewModel(usuario, this);
        });

        IrInicioCommand = new RelayCommand(_ => navSvc.NavigateTo<InicioViewModel>());
        IrHistorialCommand = new RelayCommand(_ => navSvc.NavigateTo<HistorialViewModel>());
        IrBilleteraCommand = new RelayCommand(_ => navSvc.NavigateTo<BilleteraViewModel>());
        IrMisBonosCommand = new RelayCommand(_ => navSvc.NavigateTo<MisBonosViewModel>());
        IrSoporteCommand = new RelayCommand(_ => navSvc.NavigateTo<SoporteViewModel>());
        CerrarSesionCommand = new RelayCommand(_ => CerrarSesion());

        navSvc.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(CurrentViewModel))
                OnPropertyChanged(nameof(CurrentViewModel));
        };

        _navigationService = navSvc;
        ActualizarSaldo();
        navSvc.NavigateTo<InicioViewModel>();
    }

    public void ActualizarSaldo()
    {
        try
        {
            Usuario? u = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
            if (u != null) _usuario.Saldo = u.Saldo;
        }
        catch { }
        SaldoTexto = $"Saldo: ${_usuario.Saldo:N2}";
    }

    private void CerrarSesion()
    {
        try
        {
            _logSvc.Registrar("logout", "INFO", "AUTH", "Sesión cerrada", _usuario.IdUsuario);
        }
        catch { }

        foreach (Window w in Application.Current.Windows)
        {
            if (w is MainWindow)
            {
                LoginWindow login = new LoginWindow();
                login.Show();
                w.Close();
                break;
            }
        }
    }
}
