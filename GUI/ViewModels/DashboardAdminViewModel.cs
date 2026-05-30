using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BLL;
using ENTITY;
using GUI.Commands;

namespace GUI.ViewModels;

public class DashboardAdminViewModel : ViewModelBase
{
    private readonly AdminServicio _adminSvc = new();
    private readonly PartidaServicio _partidaSvc = new();

    private int _totalUsuarios;
    private int _usuariosActivos;
    private int _partidasHoy;
    private int _partidasTotal;
    private decimal _gananciaCasaHoy;
    private decimal _gananciaCasaTotal;
    private decimal _ingresosHoy;
    private decimal _ingresosTotal;
    private decimal _promedioApuesta;
    private string _juegoMasJugado;
    private string _usuarioMasActivo;
    private bool _isCargando;

    public int TotalUsuarios { get => _totalUsuarios; set => SetProperty(ref _totalUsuarios, value); }
    public int UsuariosActivos { get => _usuariosActivos; set => SetProperty(ref _usuariosActivos, value); }
    public int PartidasHoy { get => _partidasHoy; set => SetProperty(ref _partidasHoy, value); }
    public int PartidasTotal { get => _partidasTotal; set => SetProperty(ref _partidasTotal, value); }
    public decimal GananciaCasaHoy { get => _gananciaCasaHoy; set => SetProperty(ref _gananciaCasaHoy, value); }
    public decimal GananciaCasaTotal { get => _gananciaCasaTotal; set => SetProperty(ref _gananciaCasaTotal, value); }
    public decimal IngresosHoy { get => _ingresosHoy; set => SetProperty(ref _ingresosHoy, value); }
    public decimal IngresosTotal { get => _ingresosTotal; set => SetProperty(ref _ingresosTotal, value); }
    public decimal PromedioApuesta { get => _promedioApuesta; set => SetProperty(ref _promedioApuesta, value); }
    public string JuegoMasJugado { get => _juegoMasJugado; set => SetProperty(ref _juegoMasJugado, value); }
    public string UsuarioMasActivo { get => _usuarioMasActivo; set => SetProperty(ref _usuarioMasActivo, value); }
    public bool IsCargando { get => _isCargando; set => SetProperty(ref _isCargando, value); }

    public ObservableCollection<EstadisticasUsuario> TopJugadores { get; } = new();
    public ObservableCollection<RentabilidadItem> RentabilidadJuegos { get; } = new();
    public ObservableCollection<PartidaDisplayDto> UltimasPartidas { get; } = new();

    public RelayCommand RefrescarCommand { get; }

    public DashboardAdminViewModel()
    {
        RefrescarCommand = new RelayCommand(async _ => await CargarDatosAsync());
        _ = CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        if (IsCargando) return;
        IsCargando = true;

        try
        {
            await Task.Run(() =>
            {
                ResumenAdmin resumen = _adminSvc.ObtenerResumenGeneral();
                IList<EstadisticasUsuario> top = _adminSvc.ObtenerTopJugadores(5);
                var rentabilidad = _adminSvc.ObtenerRentabilidadPorJuego();
                IList<PartidaDisplayDto> ultimas = _partidaSvc.ObtenerTodasConNombres().Take(10).ToList();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    TotalUsuarios = resumen.TotalUsuarios;
                    UsuariosActivos = resumen.UsuariosActivos;
                    PartidasHoy = resumen.PartidasHoy;
                    PartidasTotal = resumen.PartidasTotal;
                    GananciaCasaHoy = resumen.GananciaCasaHoy;
                    GananciaCasaTotal = resumen.GananciaCasaTotal;
                    IngresosHoy = resumen.IngresosHoy;
                    IngresosTotal = resumen.IngresosTotal;
                    PromedioApuesta = resumen.PromedioApuesta;
                    JuegoMasJugado = resumen.JuegoMasJugado;
                    UsuarioMasActivo = resumen.UsuarioMasActivo;

                    TopJugadores.Clear();
                    foreach (var j in top)
                        TopJugadores.Add(j);

                    RentabilidadJuegos.Clear();
                    foreach (var r in rentabilidad)
                        RentabilidadJuegos.Add(new RentabilidadItem
                        {
                            Juego = r.Juego,
                            Partidas = r.Partidas,
                            GananciaCasa = r.GananciaCasa,
                            TotalApostado = r.TotalApostado,
                            Margen = r.Margen
                        });

                    UltimasPartidas.Clear();
                    foreach (var p in ultimas)
                        UltimasPartidas.Add(p);
                });
            });
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
                MessageBox.Show($"Error al cargar dashboard: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error));
        }
        finally
        {
            IsCargando = false;
        }
    }
}

public class RentabilidadItem
{
    public string Juego { get; set; }
    public int Partidas { get; set; }
    public decimal TotalApostado { get; set; }
    public decimal GananciaCasa { get; set; }
    public decimal Margen { get; set; }
}
