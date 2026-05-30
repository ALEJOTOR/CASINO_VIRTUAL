using System.Collections.ObjectModel;
using System.Windows;
using BLL;
using ENTITY;
using GUI.Commands;

namespace GUI.ViewModels;

public class CeldaViewModel : ViewModelBase
{
    private string _texto = "";
    private string _colorFondo = "#1C3748";
    private string _colorTexto = "#FFFFFF";
    private bool _habilitada;
    private bool _esMina;
    private int _indice;

    public string Texto { get => _texto; set => SetProperty(ref _texto, value); }
    public string ColorFondo { get => _colorFondo; set => SetProperty(ref _colorFondo, value); }
    public string ColorTexto { get => _colorTexto; set => SetProperty(ref _colorTexto, value); }
    public bool Habilitada { get => _habilitada; set => SetProperty(ref _habilitada, value); }
    public bool EsMina { get => _esMina; set => SetProperty(ref _esMina, value); }
    public int Indice { get => _indice; set => SetProperty(ref _indice, value); }
}

public class MultiplicadorChip : ViewModelBase
{
    private string _texto = "";
    private string _colorFondo = "#142030";
    private string _colorTexto = "#F1F5F9";
    private bool _esSiguiente;

    public string Texto { get => _texto; set => SetProperty(ref _texto, value); }
    public string ColorFondo { get => _colorFondo; set => SetProperty(ref _colorFondo, value); }
    public string ColorTexto { get => _colorTexto; set => SetProperty(ref _colorTexto, value); }
    public bool EsSiguiente { get => _esSiguiente; set { _esSiguiente = value; OnPropertyChanged(); } }
}

public class BotonOpcion : ViewModelBase
{
    private string _texto = "";
    private bool _activo;
    private bool _permitido = true;
    public int Valor { get; set; }

    public string Texto { get => _texto; set => SetProperty(ref _texto, value); }
    public bool Activo { get => _activo; set => SetProperty(ref _activo, value); }
    public bool Permitido { get => _permitido; set => SetProperty(ref _permitido, value); }
}

public class MinasViewModel : ViewModelBase
{
    private readonly Usuario _usuario;
    private readonly PartidaServicio _servicio = new();
    private readonly UsuarioServicio _usuarioSvc = new();
    private readonly Random _random = new();

    private int _filas = 5;
    private int _cols = 5;
    private int _total = 25;
    private int _nMinas;
    private int _destapadas;
    private decimal _apuesta;
    private decimal _multiplicador = 1m;
    private bool _activa;
    private string _estadoTexto = "Configura y presiona Apostar.";
    private string _multiplicadorTexto = "Multiplicador: x1.00";
    private string _saldoTexto = "";
    private string _apuestaTexto = "1000";
    private string _minasTexto = "5";
    private string _botonAccionTexto = "Apostar";
    private bool _apuestaControlsEnabled = true;
    private bool _minasControlsEnabled = true;
    private bool _cuadriculaControlsEnabled = true;
    private bool _partidaActiva;
    private bool _dialogoPersonalVisible;
    private string _dialogoPersonalInput = "5";
    private bool[,] _esMina = new bool[9, 9];

    private ObservableCollection<CeldaViewModel> _celdas = new();
    public ObservableCollection<CeldaViewModel> Celdas
    {
        get => _celdas;
        set => SetProperty(ref _celdas, value);
    }
    public ObservableCollection<MultiplicadorChip> Multiplicadores { get; } = new();
    public ObservableCollection<BotonOpcion> BotonesMinas { get; } = new();
    public ObservableCollection<BotonOpcion> BotonesCuadricula { get; } = new();

    public string EstadoTexto { get => _estadoTexto; set => SetProperty(ref _estadoTexto, value); }
    public string MultiplicadorTexto { get => _multiplicadorTexto; set => SetProperty(ref _multiplicadorTexto, value); }
    public string SaldoTexto { get => _saldoTexto; set => SetProperty(ref _saldoTexto, value); }
    public string ApuestaTexto { get => _apuestaTexto; set => SetProperty(ref _apuestaTexto, value); }
    public string MinasTexto { get => _minasTexto; set { SetProperty(ref _minasTexto, value); ActualizarBarraMultiplicadores(); MarcarBotonMinasDesdeTexto(); } }
    public string BotonAccionTexto { get => _botonAccionTexto; set => SetProperty(ref _botonAccionTexto, value); }
    public bool ApuestaControlsEnabled { get => _apuestaControlsEnabled; set => SetProperty(ref _apuestaControlsEnabled, value); }
    public bool MinasControlsEnabled { get => _minasControlsEnabled; set => SetProperty(ref _minasControlsEnabled, value); }
    public bool CuadriculaControlsEnabled { get => _cuadriculaControlsEnabled; set => SetProperty(ref _cuadriculaControlsEnabled, value); }
    public bool PartidaActiva { get => _partidaActiva; set => SetProperty(ref _partidaActiva, value); }
    public bool DialogoPersonalVisible { get => _dialogoPersonalVisible; set => SetProperty(ref _dialogoPersonalVisible, value); }
    public string DialogoPersonalInput { get => _dialogoPersonalInput; set => SetProperty(ref _dialogoPersonalInput, value); }
    public int Filas => _filas;
    public int Columnas => _cols;

    public RelayCommand DestaparCeldaCommand { get; }
    public RelayCommand AccionPrincipalCommand { get; }
    public RelayCommand SeleccionarMinasCommand { get; }
    public RelayCommand SeleccionarCuadriculaCommand { get; }
    public RelayCommand AjustarApuestaCommand { get; }
    public RelayCommand AbrirDialogoPersonalCommand { get; }
    public RelayCommand AceptarDialogoPersonalCommand { get; }
    public RelayCommand CancelarDialogoPersonalCommand { get; }

    public event EventHandler? SaldoActualizado;

    public MinasViewModel(Usuario usuario)
    {
        _usuario = usuario;
        DestaparCeldaCommand = new RelayCommand(DestaparCelda);
        AccionPrincipalCommand = new RelayCommand(_ => AccionPrincipal());
        SeleccionarMinasCommand = new RelayCommand(SeleccionarMinas);
        SeleccionarCuadriculaCommand = new RelayCommand(SeleccionarCuadricula);
        AjustarApuestaCommand = new RelayCommand(AjustarApuesta);
        AbrirDialogoPersonalCommand = new RelayCommand(_ => DialogoPersonalVisible = true);
        AceptarDialogoPersonalCommand = new RelayCommand(_ => AceptarPersonal());
        CancelarDialogoPersonalCommand = new RelayCommand(_ => DialogoPersonalVisible = false);
        InicializarControles();
        ActualizarSaldo();
    }

    private void InicializarControles()
    {
        CrearBotonesCuadricula();
        ReconstruirBotonesMinas();
        InicializarTablero();
        ActualizarBarraMultiplicadores();
    }

    private void InicializarTablero()
    {
        var nuevas = new ObservableCollection<CeldaViewModel>();
        for (int i = 0; i < _total; i++)
        {
            nuevas.Add(new CeldaViewModel
            {
                Indice = i,
                Texto = "",
                ColorFondo = "#1C3748",
                Habilitada = true  // Las celdas SIEMPRE se inicializan habilitadas
            });
        }
        Celdas = nuevas;
    }

    private void CrearTablero()
    {
        _esMina = new bool[_filas, _cols];
        InicializarTablero();
        OnPropertyChanged(nameof(Filas));
        OnPropertyChanged(nameof(Columnas));
    }

    private void CrearBotonesCuadricula()
    {
        BotonesCuadricula.Clear();
        foreach (int tamano in new[] { 3, 5, 7, 9 })
        {
            var btn = new BotonOpcion { Texto = $"{tamano}x{tamano}", Valor = tamano };
            BotonesCuadricula.Add(btn);
        }
        MarcarBotonCuadricula();
    }

    private void ReconstruirBotonesMinas()
    {
        BotonesMinas.Clear();
        foreach (int minas in ObtenerMinasRecomendadas())
            BotonesMinas.Add(new BotonOpcion { Texto = $"{minas}", Valor = minas });
        BotonesMinas.Add(new BotonOpcion { Texto = "Personal", Valor = 0 });
        MarcarBotonMinasDesdeTexto();
        ActualizarBotonesMinasPermitidos();
    }

    private int[] ObtenerMinasRecomendadas()
    {
        if (_total <= 9) return new[] { 1, 2, 3, 5 };
        if (_total <= 25) return new[] { 1, 3, 5, 7 };
        if (_total <= 49) return new[] { 3, 5, 7, 10 };
        return new[] { 1, 3, 5, 10 };
    }

    private int MinasMinimasPorCuadricula() => 1;

    private void AjustarMinasAlTamano()
    {
        int minimo = MinasMinimasPorCuadricula();
        if (!int.TryParse(MinasTexto, out int minasActuales) || minasActuales < minimo)
            MinasTexto = minimo.ToString();
        if (int.TryParse(MinasTexto, out minasActuales) && minasActuales >= _total)
            MinasTexto = (_total - 1).ToString();
        MarcarBotonMinasDesdeTexto();
        ActualizarBotonesMinasPermitidos();
    }

    private void SeleccionarMinas(object? parameter)
    {
        if (_activa) return;
        if (parameter is not int minas) return;

        if (minas == 0)
        {
            DialogoPersonalInput = MinasTexto;
            DialogoPersonalVisible = true;
            return;
        }

        if (minas < MinasMinimasPorCuadricula())
        {
            MessageBox.Show($"En tablero {_filas}x{_cols} debes usar mínimo {MinasMinimasPorCuadricula()} minas.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        MinasTexto = Math.Min(minas, _total - 1).ToString();
        MarcarBotonMinasDesdeTexto();
        ActualizarBarraMultiplicadores();
    }

    private void AceptarPersonal()
    {
        DialogoPersonalVisible = false;
        if (!int.TryParse(DialogoPersonalInput, out int minas) || minas < 1 || minas >= _total)
        {
            MessageBox.Show($"El número de minas debe estar entre 1 y {_total - 1}.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        MinasTexto = minas.ToString();
        MarcarBotonMinasDesdeTexto();
        ActualizarBarraMultiplicadores();
    }

    private void SeleccionarCuadricula(object? parameter)
    {
        if (_activa) return;
        if (parameter is not int tamano) return;

        _filas = tamano;
        _cols = tamano;
        _total = tamano * tamano;

        ReconstruirBotonesMinas();
        AjustarMinasAlTamano();
        MarcarBotonCuadricula();
        CrearTablero();
        ActualizarBarraMultiplicadores();
    }

    private void AjustarApuesta(object? parameter)
    {
        if (_activa) return;
        
        int direccion = 0;
        if (parameter is int dir)
            direccion = dir;
        else if (parameter is string str && int.TryParse(str, out var parsed))
            direccion = parsed;
        else
            return;

        if (!decimal.TryParse(ApuestaTexto, out decimal valor))
            valor = 1000m;

        decimal paso = valor < 5000m ? 500m : 1000m;
        valor = Math.Max(500m, Math.Min(valor + direccion * paso, _usuario.Saldo));
        ApuestaTexto = valor.ToString("0");
    }

    private void AccionPrincipal()
    {
        if (_activa)
        {
            CobrarPartidaActiva();
            return;
        }
        IniciarPartida();
    }

    private void IniciarPartida()
    {
        if (!decimal.TryParse(ApuestaTexto, out _apuesta) || _apuesta <= 0)
        {
            MessageBox.Show("Ingresa una apuesta válida mayor a $0.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (_apuesta > _usuario.Saldo)
        {
            MessageBox.Show($"Saldo insuficiente. Tu saldo es ${_usuario.Saldo:N2}.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            ApuestaTexto = _usuario.Saldo.ToString("F2");
            return;
        }

        if (!int.TryParse(MinasTexto, out _nMinas) || _nMinas < 1 || _nMinas >= _total)
        {
            MessageBox.Show($"El número de minas debe estar entre 1 y {_total - 1}.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        _nMinas = Math.Min(_nMinas, _total - 1);
        MinasTexto = _nMinas.ToString();
        MarcarBotonMinasDesdeTexto();

        _destapadas = 0;
        _multiplicador = ObtenerMultiplicador(0);
        _activa = true;
        PartidaActiva = true;

        ActualizarMultiplicador();
        ActualizarBarraMultiplicadores();
        EstadoTexto = "¡Elige una celda!";

        ApuestaControlsEnabled = false;
        MinasControlsEnabled = false;
         CuadriculaControlsEnabled = false;
         BotonAccionTexto = "Cobrar";


        ColocarMinas();
        HabilitarCeldas(true);
    }

    private void ColocarMinas()
    {
        _esMina = new bool[_filas, _cols];
        var usadas = new List<int>();

        while (usadas.Count < _nMinas)
        {
            int p = _random.Next(_total);
            if (!usadas.Contains(p)) usadas.Add(p);
        }

        foreach (int p in usadas)
            _esMina[p / _cols, p % _cols] = true;

        for (int i = 0; i < _total; i++)
        {
            Celdas[i].Texto = "";
            Celdas[i].ColorFondo = "#1C3748";
            Celdas[i].Habilitada = false;
            Celdas[i].EsMina = _esMina[i / _cols, i % _cols];
        }
    }

    private void DestaparCelda(object? parameter)
    {
        if (!_activa || parameter is not int idx) return;
        if (!Celdas[idx].Habilitada) return;

        int fila = idx / _cols;
        int col = idx % _cols;

        if (_esMina[fila, col])
        {
            Celdas[idx].Texto = "✕";
            Celdas[idx].ColorFondo = "#DC2626";
            Celdas[idx].Habilitada = false;
            TerminarPartida(false);
        }
        else
        {
            _destapadas++;
            Celdas[idx].Texto = "★";
            Celdas[idx].ColorFondo = "#22C55E";
            Celdas[idx].Habilitada = false;

            _multiplicador = ObtenerMultiplicador(_destapadas);
            ActualizarMultiplicador();
            ActualizarBarraMultiplicadores();

            decimal proyectado = Math.Round(_apuesta * _multiplicador, 2);
            EstadoTexto = $"Ganancia potencial: ${proyectado:N2}";
            BotonAccionTexto = $"Cobrar ${proyectado:N2}";

            if (_destapadas >= _total - _nMinas)
                TerminarPartida(true);
        }
    }

    private void CobrarPartidaActiva()
    {
        if (!_activa) return;

        if (_destapadas == 0)
        {
            MessageBox.Show("Destapa al menos una celda antes de retirar.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        decimal proyectado = Math.Round(_apuesta * _multiplicador, 2);
        var result = MessageBox.Show(
            $"¿Deseas retirar ${proyectado:N2}?\n\nSi continúas jugando podrías ganar más, pero también perder todo.",
            "Confirmar retiro",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
            TerminarPartida(true);
    }

    private void TerminarPartida(bool gano)
    {
        _activa = false;
        PartidaActiva = false;
        HabilitarCeldas(false);
        MostrarMinas();

        decimal ganancia = gano ? Math.Round(_apuesta * _multiplicador, 2) : 0m;

        Partida p = new Partida
        {
            IdUsuario = _usuario.IdUsuario,
            IdJuego = _servicio.ObtenerIdJuegoPorNombre("Minas"),
            IdEstado = gano ? 2 : 3,
            Apuesta = _apuesta,
            Ganancia = ganancia
        };

        if (p.IdJuego == 0)
        {
            MessageBox.Show("No se encontró el juego Minas en la base de datos.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            RestablecerControles();
            return;
        }

        var (resultado, _, bonusExtra) = _servicio.RegistrarPartida(p);
        if (resultado != "Guardado correctamente.")
        {
            MessageBox.Show($"Error al registrar partida: {resultado}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            RestablecerControles();
            return;
        }

        if (bonusExtra > 0)
            MessageBox.Show($"¡Bono activo! Ganaste ${bonusExtra:N2} extra.", "¡Bono Aplicado!",
                MessageBoxButton.OK, MessageBoxImage.Information);

        Usuario? actualizado = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
        if (actualizado != null) _usuario.Saldo = actualizado.Saldo;
        ActualizarSaldo();
        SaldoActualizado?.Invoke(this, EventArgs.Empty);
        ActualizarBarraMultiplicadores();

        if (gano)
        {
            EstadoTexto = $"¡Ganaste ${ganancia:N2}!";
            MessageBox.Show($"¡Felicidades!\n\nGanaste ${ganancia:N2} con un multiplicador de x{_multiplicador:N2}.\nTu nuevo saldo: ${_usuario.Saldo:N2}",
                "¡Victoria!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            EstadoTexto = $"¡Mina! Perdiste ${_apuesta:N2}";
            MessageBox.Show($"¡Encontraste una mina!\n\nPerdiste ${_apuesta:N2}.\nTu nuevo saldo: ${_usuario.Saldo:N2}",
                "Perdiste", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        RestablecerControles();
    }

    private void MostrarMinas()
    {
        for (int i = 0; i < _filas; i++)
            for (int j = 0; j < _cols; j++)
            {
                int idx = i * _cols + j;
                if (_esMina[i, j] && string.IsNullOrEmpty(Celdas[idx].Texto))
                {
                    Celdas[idx].Texto = "✕";
                    Celdas[idx].ColorFondo = "#DC2626";
                }
            }
    }

    private void HabilitarCeldas(bool v)
    {
        for (int i = 0; i < _total; i++)
            Celdas[i].Habilitada = v;
    }

    private void RestablecerControles()
    {
        BotonAccionTexto = "Apostar";
        ApuestaControlsEnabled = true;
        MinasControlsEnabled = true;
         CuadriculaControlsEnabled = true;
         ActualizarBarraMultiplicadores();
     }


    private void MarcarBotonMinasDesdeTexto()
    {
        bool coincideRapido = false;
        foreach (var btn in BotonesMinas)
        {
            bool activo = btn.Valor > 0 && btn.Valor.ToString() == MinasTexto;
            if (activo) coincideRapido = true;
            btn.Activo = activo;
        }

        if (!coincideRapido)
        {
            var personal = BotonesMinas.FirstOrDefault(b => b.Valor == 0);
            if (personal != null) personal.Activo = true;
        }
    }

    private void MarcarBotonCuadricula()
    {
        foreach (var btn in BotonesCuadricula)
            btn.Activo = btn.Valor == _filas;
    }

    private void ActualizarBotonesMinasPermitidos()
    {
        int minimo = MinasMinimasPorCuadricula();
        foreach (var btn in BotonesMinas)
        {
            if (btn.Valor == 0) { btn.Permitido = true; continue; }
            btn.Permitido = btn.Valor >= minimo;
        }
    }

    private void ActualizarBarraMultiplicadores()
    {
        Multiplicadores.Clear();

        // Usar _nMinas durante partida activa, o parsear MinasTexto antes de partida
        int minas = _activa ? _nMinas : 1;
        if (!_activa && !int.TryParse(MinasTexto, out minas))
            minas = 1;
        minas = Math.Max(1, Math.Min(minas, _total - 1));

        int seguras = _total - minas;
        int inicio = _activa ? _destapadas + 1 : 1;
        int fin = Math.Min(seguras, inicio + 5);

        for (int reveladas = inicio; reveladas <= fin; reveladas++)
        {
            decimal mult = CalcularMultiplicador(_total, minas, reveladas);
            bool siguiente = reveladas == _destapadas + 1 && _activa;
            Multiplicadores.Add(new MultiplicadorChip
            {
                Texto = $"{mult:N2}x",
                ColorFondo = siguiente ? "#FBBF24" : "#142030",
                ColorTexto = siguiente ? "#111827" : "#F1F5F9",
                EsSiguiente = siguiente
            });
        }
    }

    private decimal ObtenerMultiplicador(int reveladas)
    {
        if (reveladas <= 0) return 1m;
        return CalcularMultiplicador(_total, _nMinas, reveladas);
    }

    private decimal CalcularMultiplicador(int total, int minas, int reveladas)
    {
        if (reveladas <= 0) return 1m;

        int seguras = total - minas;
        if (seguras <= 0 || reveladas > seguras) return 0m;

        decimal probabilidad = 1m;
        for (int i = 0; i < reveladas; i++)
            probabilidad *= (seguras - i) / (decimal)(total - i);

        if (probabilidad <= 0m) return 1000m;  // Máximo en caso de probabilidad 0
        return Math.Round(0.96m / probabilidad, 2, MidpointRounding.AwayFromZero);
    }

    private void ActualizarMultiplicador()
    {
        MultiplicadorTexto = $"Multiplicador: x{_multiplicador:N2}";
    }

    private void ActualizarSaldo()
    {
        SaldoTexto = $"Saldo disponible: ${_usuario.Saldo:N2}";
    }
}
