using System.Windows;
using ENTITY;
using GUI.Commands;
using GUI.Services;
using GUI.Views;

namespace GUI.ViewModels;

public class AdminViewModel : ViewModelBase
{
    private readonly Usuario _admin;
    private readonly INavigationService _navigationService;
    private string _nombreAdmin = "";
    private string _activeMenu = "";

    public string NombreAdmin { get => _nombreAdmin; set => SetProperty(ref _nombreAdmin, value); }
    public string ActiveMenu { get => _activeMenu; set => SetProperty(ref _activeMenu, value); }
    public ViewModelBase? CurrentViewModel => _navigationService.CurrentViewModel;

    public RelayCommand IrDashboardCommand { get; }
    public RelayCommand IrUsuariosCommand { get; }
    public RelayCommand IrPartidasCommand { get; }
    public RelayCommand IrTransaccionesCommand { get; }
    public RelayCommand IrReportesCommand { get; }
    public RelayCommand IrBitacoraCommand { get; }
    public RelayCommand IrBonosCommand { get; }
    public RelayCommand IrGestionFinancieraCommand { get; }
    public RelayCommand CerrarSesionCommand { get; }

    public AdminViewModel(Usuario admin)
    {
        _admin = admin;
        NombreAdmin = $"Administrador: {admin.Nombre1} {admin.Apellido1}";

        _navigationService = new NavigationService(tipo =>
        {
            if (tipo == typeof(DashboardAdminViewModel)) return new DashboardAdminViewModel();
            if (tipo == typeof(GestionUsuariosViewModel)) return new GestionUsuariosViewModel();
            if (tipo == typeof(PartidasAdminViewModel)) return new PartidasAdminViewModel();
            if (tipo == typeof(TransaccionesAdminViewModel)) return new TransaccionesAdminViewModel();
            if (tipo == typeof(AdminGridViewModel)) return new AdminGridViewModel();
            if (tipo == typeof(AdminReportesViewModel)) return new AdminReportesViewModel();
            if (tipo == typeof(BitacoraViewModel)) return new BitacoraViewModel();
            if (tipo == typeof(GestionBonosViewModel)) return new GestionBonosViewModel();
            if (tipo == typeof(GestionFinancieraViewModel)) return new GestionFinancieraViewModel(admin);
            return new DashboardAdminViewModel();
        });

        IrDashboardCommand = new RelayCommand(_ => Navegar<DashboardAdminViewModel>("Dashboard"));
        IrUsuariosCommand = new RelayCommand(_ => Navegar<GestionUsuariosViewModel>("Usuarios"));
        IrPartidasCommand = new RelayCommand(_ => NavegarPartidas());
        IrTransaccionesCommand = new RelayCommand(_ => NavegarTransacciones());
        IrReportesCommand = new RelayCommand(_ => Navegar<AdminReportesViewModel>("Reportes"));
        IrBitacoraCommand = new RelayCommand(_ => Navegar<BitacoraViewModel>("Bitácora"));
        IrBonosCommand = new RelayCommand(_ => Navegar<GestionBonosViewModel>("Bonos"));
        IrGestionFinancieraCommand = new RelayCommand(_ =>
        {
            ActiveMenu = "Gestión Financiera";
            _navigationService.NavigateTo<GestionFinancieraViewModel>();
        });
        CerrarSesionCommand = new RelayCommand(_ => CerrarSesion());

        _navigationService.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(CurrentViewModel))
                OnPropertyChanged(nameof(CurrentViewModel));
        };

        _navigationService.NavigateTo<DashboardAdminViewModel>();
    }

    private void Navegar<T>(string menu) where T : ViewModelBase, new()
    {
        ActiveMenu = menu;
        _navigationService.NavigateTo<T>();
    }

    private void NavegarPartidas()
    {
        ActiveMenu = "Partidas";
        _navigationService.NavigateTo<PartidasAdminViewModel>();
    }

    private void NavegarTransacciones()
    {
        ActiveMenu = "Transacciones";
        _navigationService.NavigateTo<TransaccionesAdminViewModel>();
    }

    private void CerrarSesion()
    {
        foreach (Window w in Application.Current.Windows)
        {
            if (w is AdminWindow)
            {
                LoginWindow login = new LoginWindow();
                login.Show();
                w.Close();
                break;
            }
        }
    }
}
