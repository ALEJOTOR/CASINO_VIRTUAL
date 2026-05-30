using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BLL;
using ENTITY;
using GUI.Commands;

namespace GUI.ViewModels;

public class BitacoraViewModel : ViewModelBase
{
    private readonly LogServicio _logSvc = new();

    private DateTime _fechaDesde = DateTime.Now.AddDays(-7);
    private DateTime _fechaHasta = DateTime.Now;
    private string _nivelSeleccionado = "Todos";
    private string _tipoSeleccionado = "Todos";
    private int _paginaActual = 1;
    private int _totalPaginas = 1;
    private int _totalRegistros;
    private int _contadorInfo;
    private int _contadorWarn;
    private int _contadorError;
    private int _contadorCritical;
    private bool _isCargando;

    public ObservableCollection<LogEvento> EventosPaginados { get; } = new();
    public IList<string> Niveles { get; } = new[] { "Todos", "INFO", "WARN", "ERROR", "CRITICAL" };
    public IList<string> TiposEvento { get; } = new[]
    {
        "Todos", "login", "logout", "login_fallido", "registro_usuario",
        "bloqueo_usuario", "eliminacion_usuario", "deposito", "retiro",
        "apuesta_alta", "bono_aplicado", "bono_removido", "error_sistema",
        "acceso_admin", "cambio_contrasena", "cambio_estado"
    };

    public DateTime FechaDesde { get => _fechaDesde; set { SetProperty(ref _fechaDesde, value); } }
    public DateTime FechaHasta { get => _fechaHasta; set { SetProperty(ref _fechaHasta, value); } }
    public string NivelSeleccionado { get => _nivelSeleccionado; set { SetProperty(ref _nivelSeleccionado, value); } }
    public string TipoSeleccionado { get => _tipoSeleccionado; set { SetProperty(ref _tipoSeleccionado, value); } }
    public int PaginaActual { get => _paginaActual; set { SetProperty(ref _paginaActual, value); OnPropertyChanged(nameof(InfoPaginacion)); } }
    public int TotalPaginas { get => _totalPaginas; set { SetProperty(ref _totalPaginas, value); OnPropertyChanged(nameof(InfoPaginacion)); } }
    public int TotalRegistros { get => _totalRegistros; set { SetProperty(ref _totalRegistros, value); OnPropertyChanged(nameof(InfoPaginacion)); } }
    public int ContadorInfo { get => _contadorInfo; set => SetProperty(ref _contadorInfo, value); }
    public int ContadorWarn { get => _contadorWarn; set => SetProperty(ref _contadorWarn, value); }
    public int ContadorError { get => _contadorError; set => SetProperty(ref _contadorError, value); }
    public int ContadorCritical { get => _contadorCritical; set => SetProperty(ref _contadorCritical, value); }
    public bool IsCargando { get => _isCargando; set => SetProperty(ref _isCargando, value); }

    public string InfoPaginacion => $"Página {PaginaActual} de {TotalPaginas} ({TotalRegistros} registros)";
    public bool HayPaginaAnterior => PaginaActual > 1;
    public bool HayPaginaSiguiente => PaginaActual < TotalPaginas;

    public RelayCommand AplicarFiltroCommand { get; }
    public RelayCommand LimpiarFiltroCommand { get; }
    public RelayCommand PaginaSiguienteCommand { get; }
    public RelayCommand PaginaAnteriorCommand { get; }
    public RelayCommand IrPrimeraCommand { get; }
    public RelayCommand IrUltimaCommand { get; }

    public BitacoraViewModel()
    {
        AplicarFiltroCommand = new RelayCommand(async _ => await CargarDatosAsync());
        LimpiarFiltroCommand = new RelayCommand(_ => LimpiarFiltros());
        PaginaSiguienteCommand = new RelayCommand(_ => IrPagina(PaginaActual + 1), _ => HayPaginaSiguiente);
        PaginaAnteriorCommand = new RelayCommand(_ => IrPagina(PaginaActual - 1), _ => HayPaginaAnterior);
        IrPrimeraCommand = new RelayCommand(_ => IrPagina(1), _ => PaginaActual > 1);
        IrUltimaCommand = new RelayCommand(_ => IrPagina(TotalPaginas), _ => PaginaActual < TotalPaginas);

        _ = CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        if (IsCargando) return;
        IsCargando = true;

        try
        {
            int total = 0;
            int porPagina = 100;
            string nivel = NivelSeleccionado == "Todos" ? null : NivelSeleccionado;
            string tipo = TipoSeleccionado == "Todos" ? null : TipoSeleccionado;

            IList<LogEvento> datos = await Task.Run(() =>
                _logSvc.ObtenerFiltradosDB(nivel, tipo,
                    FechaDesde, FechaHasta.AddDays(1),
                    PaginaActual, porPagina, out total));

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                TotalRegistros = total;
                TotalPaginas = Math.Max(1, (int)Math.Ceiling((double)total / porPagina));

                ContadorInfo = datos.Count(e => e.Nivel == "INFO");
                ContadorWarn = datos.Count(e => e.Nivel == "WARN");
                ContadorError = datos.Count(e => e.Nivel == "ERROR");
                ContadorCritical = datos.Count(e => e.Nivel == "CRITICAL");

                EventosPaginados.Clear();
                foreach (var item in datos)
                    EventosPaginados.Add(item);

                OnPropertyChanged(nameof(HayPaginaAnterior));
                OnPropertyChanged(nameof(HayPaginaSiguiente));
                OnPropertyChanged(nameof(InfoPaginacion));
            });
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
                MessageBox.Show($"Error al cargar bitácora: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error));
        }
        finally
        {
            IsCargando = false;
        }
    }

    private void IrPagina(int pagina)
    {
        if (pagina < 1 || pagina > TotalPaginas) return;
        PaginaActual = pagina;
        _ = CargarDatosAsync();
    }

    private void LimpiarFiltros()
    {
        FechaDesde = DateTime.Now.AddDays(-7);
        FechaHasta = DateTime.Now;
        NivelSeleccionado = "Todos";
        TipoSeleccionado = "Todos";
        PaginaActual = 1;
        _ = CargarDatosAsync();
    }
}
