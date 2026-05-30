using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BLL;
using ENTITY;
using GUI.Commands;

namespace GUI.ViewModels;

public class TransaccionesAdminViewModel : ViewModelBase
{
    private readonly TransaccionServicio _transSvc = new();

    private IList<TransaccionDisplayDto> _todasFiltradas = new List<TransaccionDisplayDto>();

    private DateTime _fechaDesde = DateTime.Now.AddMonths(-1);
    private DateTime _fechaHasta = DateTime.Now;
    private string _tipoSeleccionado = "Todos";
    private int _paginaActual = 1;
    private int _totalPaginas = 1;
    private int _totalRegistros;
    private decimal _totalDepositos;
    private decimal _totalRetiros;
    private decimal _balanceNeto;
    private decimal _totalBonos;
    private bool _isLoading;

    public ObservableCollection<TransaccionDisplayDto> TransaccionesPaginadas { get; } = new();
    public IList<string> Tipos { get; } = new[] { "Todos", "deposito", "retiro", "bono", "ganancia", "perdida" };

    public DateTime FechaDesde { get => _fechaDesde; set { SetProperty(ref _fechaDesde, value); } }
    public DateTime FechaHasta { get => _fechaHasta; set { SetProperty(ref _fechaHasta, value); } }
    public string TipoSeleccionado { get => _tipoSeleccionado; set { SetProperty(ref _tipoSeleccionado, value); } }

    public int PaginaActual { get => _paginaActual; set { SetProperty(ref _paginaActual, value); OnPropertyChanged(nameof(InfoPaginacion)); } }
    public int TotalPaginas { get => _totalPaginas; set { SetProperty(ref _totalPaginas, value); OnPropertyChanged(nameof(InfoPaginacion)); } }
    public int TotalRegistros { get => _totalRegistros; set { SetProperty(ref _totalRegistros, value); OnPropertyChanged(nameof(InfoPaginacion)); } }

    public decimal TotalDepositos { get => _totalDepositos; set => SetProperty(ref _totalDepositos, value); }
    public decimal TotalRetiros { get => _totalRetiros; set => SetProperty(ref _totalRetiros, value); }
    public decimal BalanceNeto { get => _balanceNeto; set => SetProperty(ref _balanceNeto, value); }
    public decimal TotalBonos { get => _totalBonos; set => SetProperty(ref _totalBonos, value); }
    public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

    public string InfoPaginacion => $"Página {PaginaActual} de {TotalPaginas} ({TotalRegistros} registros)";
    public bool HayPaginaAnterior => PaginaActual > 1;
    public bool HayPaginaSiguiente => PaginaActual < TotalPaginas;

    public RelayCommand AplicarFiltroCommand { get; }
    public RelayCommand LimpiarFiltroCommand { get; }
    public RelayCommand PaginaSiguienteCommand { get; }
    public RelayCommand PaginaAnteriorCommand { get; }
    public RelayCommand IrPrimeraCommand { get; }
    public RelayCommand IrUltimaCommand { get; }

    public TransaccionesAdminViewModel()
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
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            IList<TransaccionDisplayDto> datos = await Task.Run(() =>
                _transSvc.ObtenerFiltradasConNombres(FechaDesde, FechaHasta, TipoSeleccionado));

            _todasFiltradas = datos;

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                TotalDepositos = datos.Where(t => t.Tipo.Equals("Deposito", StringComparison.OrdinalIgnoreCase)).Sum(t => t.Monto);
                TotalRetiros = datos.Where(t => t.Tipo.Equals("Retiro", StringComparison.OrdinalIgnoreCase)).Sum(t => t.Monto);
                BalanceNeto = TotalDepositos - TotalRetiros;
                TotalBonos = datos.Where(t => t.Tipo.Equals("Bono", StringComparison.OrdinalIgnoreCase)).Sum(t => t.Monto);

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
                MessageBox.Show($"Error al cargar transacciones: {ex.Message}", "Error",
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

        TransaccionesPaginadas.Clear();
        foreach (var item in pagina)
            TransaccionesPaginadas.Add(item);

        OnPropertyChanged(nameof(HayPaginaAnterior));
        OnPropertyChanged(nameof(HayPaginaSiguiente));
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
        TipoSeleccionado = "Todos";
        _ = CargarDatosAsync();
    }
}
