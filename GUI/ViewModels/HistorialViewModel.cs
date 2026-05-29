using System.Collections.ObjectModel;
using BLL;
using ENTITY;
using GUI.Commands;

namespace GUI.ViewModels;

public class HistorialViewModel : ViewModelBase
{
    private readonly int _usuarioId;
    private readonly TransaccionServicio _transSvc = new();

    // ── CATEGORÍA ──────────────────────────────
    private string _categoria = "Todos";

    // ── PAGINACIÓN ──────────────────────────────────
    private int _paginaActual = 1;
    private int _totalPaginas = 1;
    private int _totalRegistros = 0;
    private const int PageSize = 25;

    // ── FILTROS DE FECHA ─────────────────────────────
    private DateTime? _fechaDesde;
    private DateTime? _fechaHasta;

    // ── ESTADO DE CARGA ──────────────────────────────
    private bool _isLoading = false;

    // ── ESTADÍSTICAS ─────────────────────────────────
    private decimal _totalNeto = 0;
    private decimal _totalGanado = 0;
    private decimal _totalNegativo = 0;

    // ── DETALLE ──────────────────────────────────
    private MovimientoResumen? _movimientoSeleccionado;
    private bool _mostrarDetalle = false;

    // ── COLECCIONES ──────────────────────────────────
    public ObservableCollection<MovimientoResumen> Movimientos { get; } = new();

    // ── PROPIEDADES PÚBLICAS ──────────────────────────────
    public string Categoria 
    { 
        get => _categoria; 
        set => SetProperty(ref _categoria, value); 
    }

    public int PaginaActual 
    { 
        get => _paginaActual; 
        private set 
        { 
            SetProperty(ref _paginaActual, value);
            OnPropertyChanged(nameof(InfoPagina));
            ActualizarComandosPaginacion();
        } 
    }

    public int TotalPaginas 
    { 
        get => _totalPaginas; 
        private set 
        { 
            SetProperty(ref _totalPaginas, value);
            OnPropertyChanged(nameof(InfoPagina));
        } 
    }

    public int TotalRegistros 
    { 
        get => _totalRegistros; 
        private set => SetProperty(ref _totalRegistros, value); 
    }

    public string InfoPagina => $"Página {PaginaActual} de {TotalPaginas}";

    public DateTime? FechaDesde 
    { 
        get => _fechaDesde; 
        set => SetProperty(ref _fechaDesde, value); 
    }

    public DateTime? FechaHasta 
    { 
        get => _fechaHasta; 
        set => SetProperty(ref _fechaHasta, value); 
    }

    public bool IsLoading 
    { 
        get => _isLoading; 
        set => SetProperty(ref _isLoading, value); 
    }

    public decimal TotalNeto 
    { 
        get => _totalNeto; 
        private set => SetProperty(ref _totalNeto, value); 
    }

    public decimal TotalGanado 
    { 
        get => _totalGanado; 
        private set => SetProperty(ref _totalGanado, value); 
    }

    public decimal TotalNegativo 
    { 
        get => _totalNegativo; 
        private set => SetProperty(ref _totalNegativo, value); 
    }

    public MovimientoResumen? MovimientoSeleccionado 
    { 
        get => _movimientoSeleccionado; 
        set => SetProperty(ref _movimientoSeleccionado, value); 
    }

    public bool MostrarDetalle 
    { 
        get => _mostrarDetalle; 
        set => SetProperty(ref _mostrarDetalle, value); 
    }

    // ── COMANDOS ──────────────────────────────────
    public RelayCommand FiltrarTodosCommand { get; }
    public RelayCommand FiltrarApuestasCommand { get; }
    public RelayCommand FiltrarDepositosCommand { get; }
    public RelayCommand AplicarFiltroFechaCommand { get; }
    public RelayCommand LimpiarFiltroFechaCommand { get; }
    public RelayCommand PaginaAnteriorCommand { get; }
    public RelayCommand PaginaSiguienteCommand { get; }
    public RelayCommand SeleccionarMovimientoCommand { get; }
    public RelayCommand CerrarDetalleCommand { get; }

    // ── CONSTRUCTOR ──────────────────────────────────
    public HistorialViewModel(Usuario usuario)
    {
        _usuarioId = usuario.IdUsuario;

        FiltrarTodosCommand = new RelayCommand(_ => 
        { 
            Categoria = "Todos"; 
            MostrarDetalle = false; 
            _ = AplicarFiltrosAsync(); 
        });

        FiltrarApuestasCommand = new RelayCommand(_ => 
        { 
            Categoria = "Apuestas"; 
            MostrarDetalle = false; 
            _ = AplicarFiltrosAsync(); 
        });

        FiltrarDepositosCommand = new RelayCommand(_ => 
        { 
            Categoria = "Depositos"; 
            MostrarDetalle = false; 
            _ = AplicarFiltrosAsync(); 
        });

        AplicarFiltroFechaCommand = new RelayCommand(_ => _ = AplicarFiltrosAsync());
        LimpiarFiltroFechaCommand = new RelayCommand(_ => 
        { 
            FechaDesde = null; 
            FechaHasta = null; 
            _ = AplicarFiltrosAsync(); 
        });

        PaginaAnteriorCommand = new RelayCommand(_ => 
        { 
            if (PaginaActual > 1) 
            { 
                PaginaActual--; 
                _ = CargarMovimientosAsync(); 
            } 
        });

        PaginaSiguienteCommand = new RelayCommand(_ => 
        { 
            if (PaginaActual < TotalPaginas) 
            { 
                PaginaActual++; 
                _ = CargarMovimientosAsync(); 
            } 
        });

        SeleccionarMovimientoCommand = new RelayCommand(SeleccionarMovimiento);
        CerrarDetalleCommand = new RelayCommand(_ => 
        { 
            MostrarDetalle = false; 
            MovimientoSeleccionado = null; 
        });

        // Cargar datos iniciales
        _ = CargarMovimientosAsync();
    }

    // ── MÉTODOS PRIVADOS ──────────────────────────────────
    private async Task AplicarFiltrosAsync()
    {
        PaginaActual = 1;
        await CargarMovimientosAsync();
    }

    private async Task CargarMovimientosAsync()
    {
        IsLoading = true;
        MostrarDetalle = false;

        try
        {
            var resultado = await _transSvc.ObtenerMovimientosPaginadosAsync(
                _usuarioId, 
                PaginaActual, 
                PageSize, 
                Categoria, 
                FechaDesde, 
                FechaHasta);

            Movimientos.Clear();
            foreach (var m in resultado.items)
                Movimientos.Add(m);

            int total = resultado.totalCount;
            TotalRegistros = total;
            TotalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));

            // Recalcular estadísticas con los datos de la página actual
            RecalcularEstadisticas();
            
            ActualizarComandosPaginacion();
        }
        catch (Exception ex)
        {
            // Log o manejo de error silencioso
            System.Diagnostics.Debug.WriteLine($"Error cargando movimientos: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void RecalcularEstadisticas()
    {
        TotalNeto = Movimientos.Sum(m => m.Monto);
        TotalGanado = Movimientos.Where(m => m.Monto > 0).Sum(m => m.Monto);
        TotalNegativo = Math.Abs(Movimientos.Where(m => m.Monto < 0).Sum(m => m.Monto));
    }

    private void ActualizarComandosPaginacion()
    {
        ((RelayCommand)PaginaAnteriorCommand).NotifyCanExecuteChanged();
        ((RelayCommand)PaginaSiguienteCommand).NotifyCanExecuteChanged();
    }

    private void SeleccionarMovimiento(object? parameter)
    {
        if (parameter is MovimientoResumen movimiento)
        {
            MovimientoSeleccionado = movimiento;
            MostrarDetalle = true;
        }
    }
}
