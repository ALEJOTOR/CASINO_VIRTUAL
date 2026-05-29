using System.Collections.ObjectModel;
using BLL;
using ENTITY;

namespace GUI.ViewModels;

public class MisBonosViewModel : ViewModelBase
{
    private readonly Usuario _usuario;
    private readonly BonoServicio _bonoSvc = new();

    public ObservableCollection<Bono> BonosDisponibles { get; } = new();
    public ObservableCollection<UsuarioBono> HistorialBonos { get; } = new();

    public MisBonosViewModel(Usuario usuario)
    {
        _usuario = usuario;
        CargarDatos();
    }

    private void CargarDatos()
    {
        try
        {
            var bonos = _bonoSvc.ObtenerBonosActivos();
            BonosDisponibles.Clear();
            foreach (var b in bonos)
                BonosDisponibles.Add(b);
        }
        catch { }

        try
        {
            var historial = _bonoSvc.ObtenerHistorialBonos(_usuario.IdUsuario);
            HistorialBonos.Clear();
            foreach (var h in historial)
                HistorialBonos.Add(h);
        }
        catch { }
    }
}
