using System;
using System.Threading.Tasks;
using System.Windows;
using BLL;
using GUI.Commands;

namespace GUI.ViewModels;

public class AdminReportesViewModel : ViewModelBase
{
    private readonly AdminServicio _adminSvc = new();

    private string _reporteActual = "";
    private bool _isGenerando;
    private string _tipoReporteSeleccionado = "Usuarios";
    private DateTime _fechaDesde = DateTime.Now.AddMonths(-1);
    private DateTime _fechaHasta = DateTime.Now;

    public string ReporteActual { get => _reporteActual; set => SetProperty(ref _reporteActual, value); }
    public bool IsGenerando { get => _isGenerando; set { SetProperty(ref _isGenerando, value); } }
    public string TipoReporteSeleccionado { get => _tipoReporteSeleccionado; set { SetProperty(ref _tipoReporteSeleccionado, value); OnPropertyChanged(nameof(RequiereFecha)); } }
    public DateTime FechaDesde { get => _fechaDesde; set => SetProperty(ref _fechaDesde, value); }
    public DateTime FechaHasta { get => _fechaHasta; set => SetProperty(ref _fechaHasta, value); }

    public IList<string> TiposReporte { get; } = new[] { "Usuarios", "Financiero", "Partidas", "Bonos" };
    public bool RequiereFecha => TipoReporteSeleccionado is "Financiero" or "Partidas";

    public RelayCommand GenerarReporteCommand { get; }
    public RelayCommand CopiarPortapapelesCommand { get; }
    public RelayCommand LimpiarCommand { get; }

    public AdminReportesViewModel()
    {
        GenerarReporteCommand = new RelayCommand(async _ => await GenerarReporteAsync(), _ => !IsGenerando);
        CopiarPortapapelesCommand = new RelayCommand(_ => CopiarPortapapeles(), _ => !string.IsNullOrEmpty(ReporteActual));
        LimpiarCommand = new RelayCommand(_ => Limpiar());
    }

    private async Task GenerarReporteAsync()
    {
        if (IsGenerando) return;
        IsGenerando = true;

        try
        {
            string reporte = await Task.Run(() =>
            {
                return TipoReporteSeleccionado switch
                {
                    "Usuarios" => _adminSvc.GenerarReporteUsuarios(),
                    "Financiero" => _adminSvc.GenerarReporteFinanciero(FechaDesde, FechaHasta),
                    "Partidas" => _adminSvc.GenerarReportePartidas(FechaDesde, FechaHasta),
                    "Bonos" => _adminSvc.GenerarReporteBonos(),
                    _ => "Tipo de reporte no válido."
                };
            });

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                ReporteActual = reporte;
            });
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
                MessageBox.Show($"Error al generar reporte: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error));
        }
        finally
        {
            IsGenerando = false;
        }
    }

    private void CopiarPortapapeles()
    {
        try
        {
            Clipboard.SetText(ReporteActual);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al copiar al portapapeles: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Limpiar()
    {
        ReporteActual = "";
        TipoReporteSeleccionado = "Usuarios";
        FechaDesde = DateTime.Now.AddMonths(-1);
        FechaHasta = DateTime.Now;
    }
}
