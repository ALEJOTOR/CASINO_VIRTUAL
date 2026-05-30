using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BLL;
using ENTITY;
using GUI.Commands;

namespace GUI.ViewModels;

public class GestionBonosViewModel : ViewModelBase
{
    private readonly BonoServicio _bonoSvc = new();
    private readonly UsuarioServicio _usuarioSvc = new();

    // Tab 1: Catalogo
    private ObservableCollection<Bono> _bonos = new();
    private Bono _bonoSeleccionado;
    private bool _modoEdicion;
    private string _nombreForm = "";
    private string _tipoForm = "";
    private string _descripcionForm = "";
    private decimal _valorForm;
    private DateTime _fechaInicioForm = DateTime.Today;
    private DateTime? _fechaFinForm;
    private bool _tieneExpiracion;
    private bool _activoForm = true;
    private int _totalBonosCatalogo;
    private int _bonosActivos;

    // Tab 2: Asignados
    private ObservableCollection<UsuarioBono> _bonosAsignados = new();
    private UsuarioBono _bonoAsignadoSeleccionado;
    private string _estadoFiltro = "Todos";
    private string _textoBusqueda = "";
    private IList<Usuario> _usuariosDisponibles = new List<Usuario>();
    private IList<Bono> _bonosDisponibles = new List<Bono>();
    private int _idUsuarioAsignar;
    private int _idBonoAsignar;
    private decimal _montoAsignar;
    private DateTime? _fechaExpiracionAsignar;
    private string _descripcionAsignar = "";

    // Tab 3: Resumen
    private decimal _montoTotalBonificado;
    private int _totalUsuariosConBono;
    private ObservableCollection<BonoResumenItem> _bonosPorTipo = new();

    public ObservableCollection<Bono> Bonos { get => _bonos; set => SetProperty(ref _bonos, value); }
    public Bono BonoSeleccionado { get => _bonoSeleccionado; set { SetProperty(ref _bonoSeleccionado, value); if (value != null) CargarFormEdicion(value); } }
    public bool ModoEdicion { get => _modoEdicion; set => SetProperty(ref _modoEdicion, value); }
    public string NombreForm { get => _nombreForm; set => SetProperty(ref _nombreForm, value); }
    public string TipoForm { get => _tipoForm; set => SetProperty(ref _tipoForm, value); }
    public string DescripcionForm { get => _descripcionForm; set => SetProperty(ref _descripcionForm, value); }
    public decimal ValorForm { get => _valorForm; set => SetProperty(ref _valorForm, value); }
    public DateTime FechaInicioForm { get => _fechaInicioForm; set => SetProperty(ref _fechaInicioForm, value); }
    public DateTime? FechaFinForm { get => _fechaFinForm; set => SetProperty(ref _fechaFinForm, value); }
    public bool TieneExpiracion { get => _tieneExpiracion; set { SetProperty(ref _tieneExpiracion, value); if (!value) FechaFinForm = null; OnPropertyChanged(nameof(FinVisible)); } }
    public bool ActivoForm { get => _activoForm; set => SetProperty(ref _activoForm, value); }
    public bool FinVisible => TieneExpiracion;
    public int TotalBonosCatalogo { get => _totalBonosCatalogo; set => SetProperty(ref _totalBonosCatalogo, value); }
    public int BonosActivosCount { get => _bonosActivos; set => SetProperty(ref _bonosActivos, value); }

    public ObservableCollection<UsuarioBono> BonosAsignados { get => _bonosAsignados; set => SetProperty(ref _bonosAsignados, value); }
    public UsuarioBono BonoAsignadoSeleccionado { get => _bonoAsignadoSeleccionado; set => SetProperty(ref _bonoAsignadoSeleccionado, value); }
    public string EstadoFiltro { get => _estadoFiltro; set { SetProperty(ref _estadoFiltro, value); _ = CargarBonosAsignadosAsync(); } }
    public string TextoBusqueda { get => _textoBusqueda; set { SetProperty(ref _textoBusqueda, value); _ = CargarBonosAsignadosAsync(); } }
    public IList<Usuario> UsuariosDisponibles { get => _usuariosDisponibles; set => SetProperty(ref _usuariosDisponibles, value); }
    public IList<Bono> BonosDisponibles { get => _bonosDisponibles; set => SetProperty(ref _bonosDisponibles, value); }
    public int IdUsuarioAsignar { get => _idUsuarioAsignar; set => SetProperty(ref _idUsuarioAsignar, value); }
    public int IdBonoAsignar { get => _idBonoAsignar; set => SetProperty(ref _idBonoAsignar, value); }
    public decimal MontoAsignar { get => _montoAsignar; set => SetProperty(ref _montoAsignar, value); }
    public DateTime? FechaExpiracionAsignar { get => _fechaExpiracionAsignar; set => SetProperty(ref _fechaExpiracionAsignar, value); }
    public string DescripcionAsignar { get => _descripcionAsignar; set => SetProperty(ref _descripcionAsignar, value); }

    public decimal MontoTotalBonificado { get => _montoTotalBonificado; set => SetProperty(ref _montoTotalBonificado, value); }
    public int TotalUsuariosConBono { get => _totalUsuariosConBono; set => SetProperty(ref _totalUsuariosConBono, value); }
    public ObservableCollection<BonoResumenItem> BonosPorTipo { get => _bonosPorTipo; set => SetProperty(ref _bonosPorTipo, value); }

    public IList<string> EstadosFiltro { get; } = new[] { "Todos", "aplicado", "revocado", "Vigente", "Expirado" };

    public RelayCommand NuevoBonoCommand { get; }
    public RelayCommand EditarBonoCommand { get; }
    public RelayCommand GuardarBonoCommand { get; }
    public RelayCommand DesactivarBonoCommand { get; }
    public RelayCommand CancelarCommand { get; }
    public RelayCommand AplicarBonoAUsuarioCommand { get; }
    public RelayCommand RevocarBonoCommand { get; }
    public RelayCommand AplicarFiltroAsignadosCommand { get; }

    public GestionBonosViewModel()
    {
        NuevoBonoCommand = new RelayCommand(_ => IniciarNuevoBono());
        EditarBonoCommand = new RelayCommand(_ => EditarBono(), _ => BonoSeleccionado != null);
        GuardarBonoCommand = new RelayCommand(async _ => await GuardarBonoAsync());
        DesactivarBonoCommand = new RelayCommand(async _ => await DesactivarBonoAsync(), _ => BonoSeleccionado != null);
        CancelarCommand = new RelayCommand(_ => CancelarEdicion());
        AplicarBonoAUsuarioCommand = new RelayCommand(async _ => await AplicarBonoAsync());
        RevocarBonoCommand = new RelayCommand(async _ => await RevocarBonoAsync(), _ => BonoAsignadoSeleccionado != null);
        AplicarFiltroAsignadosCommand = new RelayCommand(async _ => await CargarBonosAsignadosAsync());

        _ = CargarDatosInicialesAsync();
    }

    private async Task CargarDatosInicialesAsync()
    {
        try
        {
            var bonos = await Task.Run(() => _bonoSvc.ObtenerTodos());
            var usuarios = await Task.Run(() => _usuarioSvc.ObtenerTodos());
            var activos = await Task.Run(() => _bonoSvc.ObtenerBonosActivos());
            var resumen = await Task.Run(() => _bonoSvc.ObtenerResumen());
            var asignados = await Task.Run(() => _bonoSvc.ObtenerTodosParaAdmin());

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Bonos = new ObservableCollection<Bono>(bonos);
                TotalBonosCatalogo = bonos.Count;
                BonosActivosCount = activos.Count;

                UsuariosDisponibles = usuarios.Where(u => u.IdRol != 1).ToList();
                BonosDisponibles = activos;
                BonosAsignados = new ObservableCollection<UsuarioBono>(asignados);

                MontoTotalBonificado = resumen.montoTotal;
                TotalUsuariosConBono = resumen.usuarios;

                var porTipo = bonos.GroupBy(b => b.Tipo)
                    .Select(g => new BonoResumenItem
                    {
                        Tipo = g.Key,
                        Cantidad = g.Count(),
                        Monto = asignados.Where(a => a.TipoBono == g.Key).Sum(a => a.MontoAplicado)
                    }).ToList();
                BonosPorTipo = new ObservableCollection<BonoResumenItem>(porTipo);
            });
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error));
        }
    }

    private async Task CargarBonosAsignadosAsync()
    {
        try
        {
            var asignados = await Task.Run(() => _bonoSvc.ObtenerTodosParaAdmin());

            IEnumerable<UsuarioBono> filtrados = asignados;

            if (EstadoFiltro != "Todos")
            {
                if (EstadoFiltro == "Vigente" || EstadoFiltro == "Expirado")
                    filtrados = filtrados.Where(b => b.EstadoVigencia == EstadoFiltro);
                else
                    filtrados = filtrados.Where(b => b.Estado == EstadoFiltro);
            }

            if (!string.IsNullOrWhiteSpace(TextoBusqueda))
            {
                string busq = TextoBusqueda.ToLower();
                filtrados = filtrados.Where(b =>
                    (b.Username?.ToLower().Contains(busq) ?? false) ||
                    (b.NombreUsuario?.ToLower().Contains(busq) ?? false) ||
                    (b.NombreBono?.ToLower().Contains(busq) ?? false));
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
                BonosAsignados = new ObservableCollection<UsuarioBono>(filtrados));
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
                MessageBox.Show($"Error al filtrar bonos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error));
        }
    }

    private void IniciarNuevoBono()
    {
        ModoEdicion = true;
        BonoSeleccionado = null;
        NombreForm = "";
        TipoForm = "";
        DescripcionForm = "";
        ValorForm = 0;
        FechaInicioForm = DateTime.Today;
        FechaFinForm = null;
        TieneExpiracion = false;
        ActivoForm = true;
    }

    private void EditarBono()
    {
        if (BonoSeleccionado == null) return;
        ModoEdicion = true;
    }

    private void CargarFormEdicion(Bono bono)
    {
        if (!ModoEdicion)
        {
            NombreForm = bono.Nombre;
            TipoForm = bono.Tipo;
            DescripcionForm = bono.Descripcion;
            ValorForm = bono.Valor;
            FechaInicioForm = bono.FechaInicio;
            FechaFinForm = bono.FechaFin;
            TieneExpiracion = bono.FechaFin.HasValue;
            ActivoForm = bono.Activo;
        }
    }

    private async Task GuardarBonoAsync()
    {
        try
        {
            if (BonoSeleccionado == null)
            {
                // Crear nuevo
                var bono = new Bono
                {
                    Nombre = NombreForm,
                    Tipo = TipoForm,
                    Descripcion = DescripcionForm,
                    Valor = ValorForm,
                    FechaInicio = FechaInicioForm,
                    FechaFin = TieneExpiracion ? FechaFinForm : null,
                    Activo = ActivoForm,
                    UsosActuales = 0
                };

                string resultado = await Task.Run(() => _bonoSvc.CrearBono(bono));
                if (resultado.Contains("correctamente"))
                {
                    await CargarDatosInicialesAsync();
                    ModoEdicion = false;
                }
                else
                {
                    MessageBox.Show(resultado, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Editar existente
                BonoSeleccionado.Nombre = NombreForm;
                BonoSeleccionado.Tipo = TipoForm;
                BonoSeleccionado.Descripcion = DescripcionForm;
                BonoSeleccionado.Valor = ValorForm;
                BonoSeleccionado.FechaInicio = FechaInicioForm;
                BonoSeleccionado.FechaFin = TieneExpiracion ? FechaFinForm : null;
                BonoSeleccionado.Activo = ActivoForm;

                string resultado = await Task.Run(() => _bonoSvc.ActualizarBono(BonoSeleccionado));
                if (resultado.Contains("correctamente"))
                {
                    await CargarDatosInicialesAsync();
                    ModoEdicion = false;
                }
                else
                {
                    MessageBox.Show(resultado, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al guardar bono: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task DesactivarBonoAsync()
    {
        if (BonoSeleccionado == null) return;

        var confirm = MessageBox.Show($"¿Desactivar el bono '{BonoSeleccionado.Nombre}'?",
            "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (confirm != MessageBoxResult.Yes) return;

        try
        {
            string resultado = await Task.Run(() => _bonoSvc.DesactivarBono(BonoSeleccionado.IdBono));
            if (resultado.Contains("correctamente"))
            {
                await CargarDatosInicialesAsync();
                ModoEdicion = false;
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al desactivar bono: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CancelarEdicion()
    {
        ModoEdicion = false;
        BonoSeleccionado = null;
    }

    private async Task AplicarBonoAsync()
    {
        if (IdUsuarioAsignar <= 0 || IdBonoAsignar <= 0)
        {
            MessageBox.Show("Seleccione un usuario y un bono.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (MontoAsignar <= 0)
        {
            MessageBox.Show("El monto debe ser mayor a 0.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            string resultado = await Task.Run(() =>
                _bonoSvc.AplicarBonoConFecha(
                    IdUsuarioAsignar, IdBonoAsignar,
                    MontoAsignar, FechaExpiracionAsignar, DescripcionAsignar));

            if (resultado.Contains("correctamente"))
            {
                await CargarBonosAsignadosAsync();
                await Task.Run(() =>
                {
                    var r = _bonoSvc.ObtenerResumen();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MontoTotalBonificado = r.montoTotal;
                        TotalUsuariosConBono = r.usuarios;
                    });
                });

                IdUsuarioAsignar = 0;
                IdBonoAsignar = 0;
                MontoAsignar = 0;
                FechaExpiracionAsignar = null;
                DescripcionAsignar = "";
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al aplicar bono: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task RevocarBonoAsync()
    {
        if (BonoAsignadoSeleccionado == null) return;

        var confirm = MessageBox.Show(
            $"¿Revocar bono '{BonoAsignadoSeleccionado.NombreBono}' de {BonoAsignadoSeleccionado.NombreUsuario}?",
            "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (confirm != MessageBoxResult.Yes) return;

        try
        {
            string resultado = await Task.Run(() =>
                _bonoSvc.RevocarBono(BonoAsignadoSeleccionado.IdUsuarioBono, "Revocado por administrador"));

            if (resultado.Contains("correctamente"))
            {
                await CargarBonosAsignadosAsync();
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al revocar bono: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

public class BonoResumenItem
{
    public string Tipo { get; set; }
    public int Cantidad { get; set; }
    public decimal Monto { get; set; }
}
