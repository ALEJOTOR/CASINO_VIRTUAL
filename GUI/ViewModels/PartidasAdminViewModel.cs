using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BLL;
using ENTITY;
using GUI.Commands;

namespace GUI.ViewModels;

public class PartidasAdminViewModel : ViewModelBase
{
    private readonly PartidaServicio _partidaSvc = new();
    private readonly JuegoServicio _juegoSvc = new();

    private IList<PartidaDisplayDto> _todasFiltradas = new List<PartidaDisplayDto>();

    private DateTime _fechaDesde = DateTime.Now.AddMonths(-1);
    private DateTime _fechaHasta = DateTime.Now;
    private int? _juegoSeleccionado;
    private string _resultadoSeleccionado = "Todos";
    private int _paginaActual = 1;
    private int _totalPaginas = 1;
    private int _totalRegistros;
    private decimal _totalApostadoFiltro;
    private decimal _totalGananciaFiltro;
    private int _totalPartidasFiltro;
    private int _totalGanadasFiltro;
    private int _totalPerdidasFiltro;
    private bool _isLoading;

    public ObservableCollection<PartidaDisplayDto> PartidasPaginadas { get; } = new();
    public IList<Juego> Juegos { get; }
    public IList<string> Resultados { get; } = new[] { "Todos", "ganada", "perdida" };

    public DateTime FechaDesde { get => _fechaDesde; set { SetProperty(ref _fechaDesde, value); } }
    public DateTime FechaHasta { get => _fechaHasta; set { SetProperty(ref _fechaHasta, value); } }
    public int? JuegoSeleccionado { get => _juegoSeleccionado; set { SetProperty(ref _juegoSeleccionado, value); } }
    public string ResultadoSeleccionado { get => _resultadoSeleccionado; set { SetProperty(ref _resultadoSeleccionado, value); } }

    public int PaginaActual { get => _paginaActual; set { SetProperty(ref _paginaActual, value); OnPropertyChanged(nameof(InfoPaginacion)); } }
    public int TotalPaginas { get => _totalPaginas; set { SetProperty(ref _totalPaginas, value); OnPropertyChanged(nameof(InfoPaginacion)); } }
    public int TotalRegistros { get => _totalRegistros; set { SetProperty(ref _totalRegistros, value); OnPropertyChanged(nameof(InfoPaginacion)); } }

    public decimal TotalApostadoFiltro { get => _totalApostadoFiltro; set => SetProperty(ref _totalApostadoFiltro, value); }
    public decimal TotalGananciaFiltro { get => _totalGananciaFiltro; set => SetProperty(ref _totalGananciaFiltro, value); }
    public int TotalPartidasFiltro { get => _totalPartidasFiltro; set => SetProperty(ref _totalPartidasFiltro, value); }
    public int TotalGanadasFiltro { get => _totalGanadasFiltro; set => SetProperty(ref _totalGanadasFiltro, value); }
    public int TotalPerdidasFiltro { get => _totalPerdidasFiltro; set => SetProperty(ref _totalPerdidasFiltro, value); }
    public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

    public string InfoPaginacion => $"Página {PaginaActual} de {TotalPaginas} ({TotalRegistros} registros)";

    public bool HayPaginaAnterior => PaginaActual > 1;
    public bool HayPaginaSiguiente => PaginaActual < TotalPaginas;
    public bool HayDatos => TotalRegistros > 0;

    public RelayCommand AplicarFiltroCommand { get; }
    public RelayCommand LimpiarFiltroCommand { get; }
    public RelayCommand PaginaSiguienteCommand { get; }
    public RelayCommand PaginaAnteriorCommand { get; }
    public RelayCommand IrPrimeraCommand { get; }
    public RelayCommand IrUltimaCommand { get; }

    public PartidasAdminViewModel()
    {
        try
        {
            Juegos = _juegoSvc.ObtenerTodos();
        }
        catch
        {
            Juegos = new List<Juego>();
        }

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
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            IList<PartidaDisplayDto> datos = await Task.Run(() =>
                _partidaSvc.ObtenerFiltradasConNombres(
                    FechaDesde, FechaHasta, JuegoSeleccionado, ResultadoSeleccionado));

            _todasFiltradas = datos;

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                TotalPartidasFiltro = datos.Count;
                TotalGanadasFiltro = datos.Count(p => p.Estado.Equals("ganada", StringComparison.OrdinalIgnoreCase));
                TotalPerdidasFiltro = datos.Count(p => p.Estado.Equals("perdida", StringComparison.OrdinalIgnoreCase));
                TotalApostadoFiltro = datos.Sum(p => p.Apuesta);
                TotalGananciaFiltro = datos.Sum(p => p.Ganancia);

                TotalRegistros = datos.Count;
                int registrosPorPagina = 50;
                TotalPaginas = Math.Max(1, (int)Math.Ceiling((double)TotalRegistros / registrosPorPagina));
                PaginaActual = 1;

                AplicarPagina();
            });
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
                MessageBox.Show($"Error al cargar partidas: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error));
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void AplicarPagina()
    {
        int registrosPorPagina = 50;
        var pagina = _todasFiltradas
            .Skip((PaginaActual - 1) * registrosPorPagina)
            .Take(registrosPorPagina)
            .ToList();

        PartidasPaginadas.Clear();
        foreach (var item in pagina)
            PartidasPaginadas.Add(item);

        OnPropertyChanged(nameof(HayPaginaAnterior));
        OnPropertyChanged(nameof(HayPaginaSiguiente));
        OnPropertyChanged(nameof(HayDatos));
        OnPropertyChanged(nameof(InfoPaginacion));
    }

    private void IrPagina(int pagina)
    {
        if (pagina < 1 || pagina > TotalPaginas) return;
        PaginaActual = pagina;
        AplicarPagina();
    }

    private void LimpiarFiltros()
    {
        FechaDesde = DateTime.Now.AddMonths(-1);
        FechaHasta = DateTime.Now;
        JuegoSeleccionado = null;
        ResultadoSeleccionado = "Todos";
        _ = CargarDatosAsync();
    }
}
