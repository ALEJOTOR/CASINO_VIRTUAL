using System.Collections.ObjectModel;
using ENTITY;

namespace GUI.ViewModels;

public class AdminGridViewModel : ViewModelBase
{
    private string _titulo = "";
    private string[]? _columnas;
    private string[]? _encabezados;

    public string Titulo { get => _titulo; set => SetProperty(ref _titulo, value); }
    public string[]? Columnas { get => _columnas; set => SetProperty(ref _columnas, value); }
    public string[]? Encabezados { get => _encabezados; set => SetProperty(ref _encabezados, value); }

    public ObservableCollection<object> Items { get; } = new();

    public AdminGridViewModel()
    {
    }

    public void CargarPartidas(IList<PartidaDisplayDto> datos)
    {
        Titulo = "Partidas - Registro completo";
        Columnas = new[] { "IdPartida", "Usuario", "Juego", "Estado", "Fecha", "Apuesta", "Ganancia" };
        Encabezados = new[] { "Partida", "Usuario", "Juego", "Estado", "Fecha", "Apuesta", "Ganancia" };
        Items.Clear();
        foreach (var item in datos) Items.Add(item);
    }

    public void CargarTransacciones(IList<TransaccionDisplayDto> datos)
    {
        Titulo = "Transacciones - Movimientos financieros";
        Columnas = new[] { "IdTransaccion", "Usuario", "Tipo", "Monto", "Fecha", "Descripcion" };
        Encabezados = new[] { "Movimiento", "Usuario", "Tipo", "Monto", "Fecha", "Descripcion" };
        Items.Clear();
        foreach (var item in datos) Items.Add(item);
    }
}
