using System.Collections.ObjectModel;
using System.Windows;
using BLL;
using ENTITY;
using GUI.Commands;

namespace GUI.ViewModels;

public class ApuestaRuletaItem : ViewModelBase
{
    private string _tipo = "";
    private int _numero;
    private decimal _monto;

    public string Tipo { get => _tipo; set => SetProperty(ref _tipo, value); }
    public int Numero { get => _numero; set => SetProperty(ref _numero, value); }
    public decimal Monto { get => _monto; set => SetProperty(ref _monto, value); }
}

public class FichaItem : ViewModelBase
{
    public decimal Valor { get; set; }
    public string Texto { get; set; } = "";
    public string ColorHex { get; set; } = "";
    private bool _seleccionada;
    public bool Seleccionada { get => _seleccionada; set => SetProperty(ref _seleccionada, value); }
}

public class FichaColocadaVisual : ViewModelBase
{
    public string Texto { get; set; } = "";
    public string ColorHex { get; set; } = "";
}

public class ZonaApuesta : ViewModelBase
{
    private string _tipo = "";
    private int _numero = -1;
    private int _fila;
    private int _columna;
    private int _rowSpan = 1;
    private int _colSpan = 1;
    private string _texto = "";
    private string _colorFondo = "Transparent";
    private string _colorTexto = "#FFFFFF";

    public string Tipo { get => _tipo; set => SetProperty(ref _tipo, value); }
    public int Numero { get => _numero; set => SetProperty(ref _numero, value); }
    public int Fila { get => _fila; set => SetProperty(ref _fila, value); }
    public int Columna { get => _columna; set => SetProperty(ref _columna, value); }
    public int RowSpan { get => _rowSpan; set => SetProperty(ref _rowSpan, value); }
    public int ColSpan { get => _colSpan; set => SetProperty(ref _colSpan, value); }
    public string Texto { get => _texto; set => SetProperty(ref _texto, value); }
    public string ColorFondo { get => _colorFondo; set => SetProperty(ref _colorFondo, value); }
    public string ColorTexto { get => _colorTexto; set => SetProperty(ref _colorTexto, value); }
    public ObservableCollection<FichaColocadaVisual> FichasColocadas { get; } = new();
    public string Key => Tipo == "Numero" ? $"{Tipo}:{Numero}" : Tipo;
}

public class RuletaViewModel : ViewModelBase
{
    private readonly Usuario _usuario;
    private readonly PartidaServicio _servicio = new();
    private readonly UsuarioServicio _usuarioSvc = new();
    private readonly ApuestaServicio _apuestaSvc = new();
    private readonly Random _random = new();

    private decimal _fichaSeleccionada = 50m;
    private string _saldoTexto = "";
    private string _resultadoTexto = "Mesa abierta. Color paga x2 | Número exacto paga x36";
    private string _resultadoColor = "#F1F5F9";
    private string _premioTexto = "Hagan sus apuestas";
    private string _numeroGanadorTexto = "--";
    private string _numeroGanadorColor = "#0F172A";
    private string _numeroGanadorNombreColor = "";
    private string _historialTexto = "";
    private string _totalApostadoTexto = "Total apostado: $0";
    private string _fichaTexto = "$50";
    private int _numeroGanador = -1;
    private bool _girando;

    public ObservableCollection<ApuestaRuletaItem> Apuestas { get; } = new();
    public ObservableCollection<FichaItem> Fichas { get; } = new();
    public ObservableCollection<ZonaApuesta> ZonasApuesta { get; } = new();

    public string SaldoTexto { get => _saldoTexto; set => SetProperty(ref _saldoTexto, value); }
    public string ResultadoTexto { get => _resultadoTexto; set => SetProperty(ref _resultadoTexto, value); }
    public string ResultadoColor { get => _resultadoColor; set => SetProperty(ref _resultadoColor, value); }
    public string PremioTexto { get => _premioTexto; set => SetProperty(ref _premioTexto, value); }
    public string NumeroGanadorTexto { get => _numeroGanadorTexto; set => SetProperty(ref _numeroGanadorTexto, value); }
    public string NumeroGanadorColor { get => _numeroGanadorColor; set => SetProperty(ref _numeroGanadorColor, value); }
    public string NumeroGanadorNombreColor { get => _numeroGanadorNombreColor; set => SetProperty(ref _numeroGanadorNombreColor, value); }
    public string HistorialTexto { get => _historialTexto; set => SetProperty(ref _historialTexto, value); }
    public string TotalApostadoTexto { get => _totalApostadoTexto; set => SetProperty(ref _totalApostadoTexto, value); }
    public string FichaTexto { get => _fichaTexto; set => SetProperty(ref _fichaTexto, value); }
    public bool Girando { get => _girando; set => SetProperty(ref _girando, value); }

    public RelayCommand GirarCommand { get; }
    public RelayCommand LimpiarCommand { get; }
    public RelayCommand SeleccionarFichaCommand { get; }
    public RelayCommand ApostarEnZonaCommand { get; }

    public event EventHandler? SaldoActualizado;

    public RuletaViewModel(Usuario usuario)
    {
        _usuario = usuario;
        GirarCommand = new RelayCommand(_ => Girar());
        LimpiarCommand = new RelayCommand(_ => LimpiarApuestas());
        SeleccionarFichaCommand = new RelayCommand(SeleccionarFicha);
        ApostarEnZonaCommand = new RelayCommand(ApostarEnZona);

        InicializarFichas();
        InicializarZonas();
        ActualizarSaldo();
        ActualizarTotalApostado();
    }

    private void InicializarFichas()
    {
        (decimal valor, string color)[] fichas =
        {
            (50m, "#78736C"), (500m, "#B45309"), (2500m, "#6B7280"),
            (5000m, "#CA8A04"), (25000m, "#B91C1C"), (100000m, "#0284C7"),
            (500000m, "#1F2937"), (5000000m, "#2563EB")
        };

        foreach (var (valor, color) in fichas)
        {
            Fichas.Add(new FichaItem
            {
                Valor = valor,
                Texto = FormatearFicha(valor),
                ColorHex = color,
                Seleccionada = valor == _fichaSeleccionada
            });
        }
    }

    private void InicializarZonas()
    {
        ZonasApuesta.Clear();

        ZonasApuesta.Add(new ZonaApuesta
        {
            Tipo = "Numero", Numero = 0, Fila = 0, Columna = 0,
            RowSpan = 3, ColSpan = 1, Texto = "0", ColorFondo = "#16A34A"
        });

        for (int n = 1; n <= 36; n++)
        {
            int fila = 2 - ((n - 1) % 3);
            int columna = ((n - 1) / 3) + 1;
            string color = ObtenerColor(n) == "Rojo" ? "#B91C1C" : "#0C0C0E";
            ZonasApuesta.Add(new ZonaApuesta
            {
                Tipo = "Numero", Numero = n,
                Fila = fila, Columna = columna,
                RowSpan = 1, ColSpan = 1,
                Texto = n.ToString(), ColorFondo = color
            });
        }

        ZonasApuesta.Add(new ZonaApuesta { Tipo = "Docena1", Fila = 3, Columna = 1, ColSpan = 4, Texto = "1-12", ColorFondo = "#064E3B" });
        ZonasApuesta.Add(new ZonaApuesta { Tipo = "Docena2", Fila = 3, Columna = 5, ColSpan = 4, Texto = "13-24", ColorFondo = "#064E3B" });
        ZonasApuesta.Add(new ZonaApuesta { Tipo = "Docena3", Fila = 3, Columna = 9, ColSpan = 4, Texto = "25-36", ColorFondo = "#064E3B" });

        ZonasApuesta.Add(new ZonaApuesta { Tipo = "1-18", Fila = 4, Columna = 1, ColSpan = 2, Texto = "1-18", ColorFondo = "#064E3B" });
        ZonasApuesta.Add(new ZonaApuesta { Tipo = "Par", Fila = 4, Columna = 3, ColSpan = 2, Texto = "PAR", ColorFondo = "#064E3B" });
        ZonasApuesta.Add(new ZonaApuesta { Tipo = "Rojo", Fila = 4, Columna = 5, ColSpan = 2, Texto = "ROJO", ColorFondo = "#B91C1C" });
        ZonasApuesta.Add(new ZonaApuesta { Tipo = "Negro", Fila = 4, Columna = 7, ColSpan = 2, Texto = "NEGRO", ColorFondo = "#0C0C0E" });
        ZonasApuesta.Add(new ZonaApuesta { Tipo = "Impar", Fila = 4, Columna = 9, ColSpan = 2, Texto = "IMPAR", ColorFondo = "#064E3B" });
        ZonasApuesta.Add(new ZonaApuesta { Tipo = "19-36", Fila = 4, Columna = 11, ColSpan = 2, Texto = "19-36", ColorFondo = "#064E3B" });
    }

    private void SeleccionarFicha(object? parameter)
    {
        if (parameter is decimal valor)
        {
            _fichaSeleccionada = valor;
            FichaTexto = FormatearFicha(valor);
            foreach (var f in Fichas)
                f.Seleccionada = f.Valor == valor;
        }
    }

    private void ApostarEnZona(object? parameter)
    {
        if (parameter is not string key) return;

        string[] parts = key.Split(':');
        string tipo = parts[0];
        int numero = parts.Length > 1 && int.TryParse(parts[1], out int n) ? n : -1;

        if (CalcularTotalApostado() + _fichaSeleccionada > _usuario.Saldo)
        {
            MessageBox.Show("No tienes saldo suficiente para colocar esa ficha.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var existente = Apuestas.FirstOrDefault(a => a.Tipo == tipo && a.Numero == numero);
        if (existente != null)
        {
            existente.Monto += _fichaSeleccionada;
            
            // Agregar ficha visual a la zona
            var zona = ZonasApuesta.FirstOrDefault(z => z.Key == key);
            if (zona != null)
            {
                var fichaSelec = Fichas.FirstOrDefault(f => f.Valor == _fichaSeleccionada);
                if (fichaSelec != null)
                {
                    zona.FichasColocadas.Add(new FichaColocadaVisual
                    {
                        Texto = fichaSelec.Texto,
                        ColorHex = fichaSelec.ColorHex
                    });
                }
            }
            
            ActualizarTotalApostado();
            return;
        }

        string? conflicto = ObtenerConflictoApuesta(tipo);
        if (!string.IsNullOrEmpty(conflicto))
        {
            MessageBox.Show($"Ya tienes una apuesta activa en {conflicto}. Limpia la mesa si quieres cambiar esa selección.",
                "Apuesta no permitida", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Apuestas.Add(new ApuestaRuletaItem
        {
            Tipo = tipo,
            Numero = numero,
            Monto = _fichaSeleccionada
        });

        // Agregar ficha visual a la zona
        var zonaApuesta = ZonasApuesta.FirstOrDefault(z => z.Key == key);
        if (zonaApuesta != null)
        {
            var fichaSeleccionada = Fichas.FirstOrDefault(f => f.Valor == _fichaSeleccionada);
            if (fichaSeleccionada != null)
            {
                zonaApuesta.FichasColocadas.Add(new FichaColocadaVisual
                {
                    Texto = fichaSeleccionada.Texto,
                    ColorHex = fichaSeleccionada.ColorHex
                });
            }
        }

        ActualizarTotalApostado();
    }

    private string? ObtenerConflictoApuesta(string tipo)
    {
        // Obtener el grupo de conflicto del tipo
        string? grupo = tipo switch
        {
            "Rojo" or "Negro" => "Color",
            "Par" or "Impar" => "Paridad",
            "1-18" or "19-36" => "Mitad",
            "Docena1" or "Docena2" or "Docena3" => "Docena",
            _ => null
        };

        if (grupo == null) return null;

        // Verificar si hay una apuesta activa en este grupo
        // Para Color: si hay "Rojo" y ahora intenta "Negro" (o viceversa)
        if (grupo == "Color")
            return Apuestas.Any(a => a.Tipo is "Rojo" or "Negro") ? "Color" : null;

        // Para Paridad: si hay "Par" y ahora intenta "Impar" (o viceversa)
        if (grupo == "Paridad")
            return Apuestas.Any(a => a.Tipo is "Par" or "Impar") ? "Paridad" : null;

        // Para Mitad: si hay "1-18" y ahora intenta "19-36" (o viceversa)
        if (grupo == "Mitad")
            return Apuestas.Any(a => a.Tipo is "1-18" or "19-36") ? "Mitad" : null;

        // Para Docena: si hay una docena y ahora intenta otra
        if (grupo == "Docena")
            return Apuestas.Any(a => a.Tipo is "Docena1" or "Docena2" or "Docena3") ? "Docena" : null;

        return null;
    }

     private async void Girar()
    {
        if (Girando) return;

        if (_usuario == null)
        {
            MessageBox.Show("Debe iniciar sesión para jugar.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        decimal totalApostado = CalcularTotalApostado();
        if (totalApostado <= 0)
        {
            MessageBox.Show("Selecciona una ficha y colócala sobre la mesa antes de girar.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (totalApostado > _usuario.Saldo)
        {
            MessageBox.Show($"Saldo insuficiente. Tu saldo es ${_usuario.Saldo:N2}.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Girando = true;
        PremioTexto = "Girando...";
        NumeroGanadorTexto = "?";
        NumeroGanadorColor = "#334155";
        NumeroGanadorNombreColor = "";

        int numero = _random.Next(0, 37);
        _numeroGanador = numero;
        string color = ObtenerColor(numero);
        decimal ganancia = 0m;

        foreach (var ap in Apuestas)
            if (EsApuestaGanadora(ap, numero, color))
                ganancia += ap.Monto * ObtenerMultiplicador(ap.Tipo);

        // Esperar 2.5 segundos con animación de "girando"
        await Task.Delay(2500);

        MostrarNumeroEnRueda(numero, color);
        RegistrarResultado(totalApostado, ganancia, ganancia > 0,
            $"Salió {numero} {color}. Fichas jugadas: {Apuestas.Count}. Total apostado: ${totalApostado:N0}.");
    }

    private void MostrarNumeroEnRueda(int numero, string color)
    {
        NumeroGanadorTexto = numero.ToString();
        (string colorHex, string nombreColor) = color switch
        {
            "Rojo" => ("#B91C1C", "ROJO"),
            "Negro" => ("#0C0C0E", "NEGRO"),
            _ => ("#16A34A", "VERDE")
        };
        NumeroGanadorColor = colorHex;
        NumeroGanadorNombreColor = nombreColor;
    }

    private void RegistrarResultado(decimal apuestaTotal, decimal ganancia, bool gano, string detalle)
    {
        try
        {
            Partida partida = new Partida
            {
                IdUsuario = _usuario.IdUsuario,
                IdJuego = _servicio.ObtenerIdJuegoPorNombre("Ruleta"),
                IdEstado = gano ? 2 : 3,
                Apuesta = apuestaTotal,
                Ganancia = ganancia
            };

            if (partida.IdJuego == 0)
            {
                MessageBox.Show("No se encontró el juego Ruleta en la base de datos. Reinicia la aplicación para inicializarlo.",
                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var (mensaje, idPartida, bonusExtra) = _servicio.RegistrarPartida(partida);
            if (mensaje != "Guardado correctamente.")
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (bonusExtra > 0)
                MessageBox.Show($"¡Bono activo! Ganaste ${bonusExtra:N2} extra.", "¡Bono Aplicado!",
                    MessageBoxButton.OK, MessageBoxImage.Information);

            if (idPartida > 0)
            {
                foreach (var ap in Apuestas)
                {
                    string tipoOracle = MapearTipoApuesta(ap.Tipo);
                    int? numeroOracle = ap.Tipo == "Numero" ? ap.Numero : (int?)null;
                    decimal gananciaApuesta = EsApuestaGanadora(ap, _numeroGanador, ObtenerColor(_numeroGanador))
                        ? ap.Monto * ObtenerMultiplicador(ap.Tipo) : 0;
                    int multiplicador = ObtenerMultiplicador(ap.Tipo);
                    string resultado = EsApuestaGanadora(ap, _numeroGanador, ObtenerColor(_numeroGanador))
                        ? "ganada" : "perdida";

                    _apuestaSvc.RegistrarApuesta(new Apuesta
                    {
                        IdPartida = idPartida,
                        TipoApuesta = tipoOracle,
                        NumeroApuesta = numeroOracle,
                        Monto = ap.Monto,
                        Multiplicador = multiplicador,
                        Ganancia = gananciaApuesta,
                        Resultado = resultado
                    });
                }
            }

            Usuario? actualizado = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
            if (actualizado != null) _usuario.Saldo = actualizado.Saldo;
            ActualizarSaldo();
            SaldoActualizado?.Invoke(this, EventArgs.Empty);

            // Actualizar feedback visual del resultado
            ResultadoTexto = detalle;
            ResultadoColor = gano ? "#22C55E" : "#EF4444";  // Verde si gana, rojo si pierde
            PremioTexto = gano ? $"¡GANASTE! ${ganancia:N2}" : $"PERDISTE ${apuestaTotal:N2}";
            
            // Construir texto del historial ANTES de limpiar la mesa
            string colorResultado = ObtenerColor(_numeroGanador); // "Rojo", "Negro" o "Verde"
            string resultadoRuleta = $"{_numeroGanador} {colorResultado}";

            string descripcionApuestas = Apuestas.Count == 1
                ? DescribirApuesta(Apuestas[0])
                : string.Join(", ", Apuestas.Select(a => DescribirApuesta(a)));

            if (gano)
                HistorialTexto = $"Apostaste ${apuestaTotal:N0} a {descripcionApuestas} y ganaste ${ganancia:N0}. Cayó el {resultadoRuleta}.";
            else
                HistorialTexto = $"Apostaste ${apuestaTotal:N0} a {descripcionApuestas} y perdiste, cayó el {resultadoRuleta}.";

            // AHORA sí limpiar solo la mesa (conserva número y feedback)
            LimpiarMesa();
        }
        finally
        {
            Girando = false;
        }
    }

    private void LimpiarMesa()
    {
        Apuestas.Clear();
        foreach (var zona in ZonasApuesta)
            zona.FichasColocadas.Clear();
        ActualizarTotalApostado();
    }

    private void LimpiarApuestas()
    {
        LimpiarMesa();
        ResultadoTexto = "Mesa limpia. Selecciona una ficha y toca la mesa.";
        ResultadoColor = "#F1F5F9";  // Reset al color por defecto
        PremioTexto = "Hagan sus apuestas";
        NumeroGanadorTexto = "--";
        NumeroGanadorColor = "#0F172A";
        HistorialTexto = "";
    }

    private static string DescribirApuesta(ApuestaRuletaItem ap) => ap.Tipo switch
    {
        "Numero" => $"número {ap.Numero}",
        "Rojo"   => "color Rojo",
        "Negro"  => "color Negro",
        "Par"    => "Par",
        "Impar"  => "Impar",
        "1-18"   => "1 al 18",
        "19-36"  => "19 al 36",
        "Docena1" => "docena 1-12",
        "Docena2" => "docena 13-24",
        "Docena3" => "docena 25-36",
        _ => ap.Tipo
    };

    private void ActualizarTotalApostado()
    {
        TotalApostadoTexto = $"Total apostado: ${CalcularTotalApostado():N0}";
    }

    private decimal CalcularTotalApostado()
    {
        decimal total = 0m;
        foreach (var ap in Apuestas)
            total += ap.Monto;
        return total;
    }

    private static bool EsApuestaGanadora(ApuestaRuletaItem ap, int numero, string color)
    {
        return ap.Tipo switch
        {
            "Rojo" => color == "Rojo",
            "Negro" => color == "Negro",
            "Docena1" => numero >= 1 && numero <= 12,
            "Docena2" => numero >= 13 && numero <= 24,
            "Docena3" => numero >= 25 && numero <= 36,
            "1-18" => numero >= 1 && numero <= 18,
            "19-36" => numero >= 19 && numero <= 36,
            "Par" => numero != 0 && numero % 2 == 0,
            "Impar" => numero % 2 == 1,
            "Numero" => numero == ap.Numero,
            _ => false
        };
    }

    private static int ObtenerMultiplicador(string tipo)
    {
        if (tipo == "Numero") return 36;
        if (tipo.StartsWith("Docena")) return 3;
        return 2;
    }

    private static string ObtenerColor(int numero)
    {
        if (numero == 0) return "Verde";
        int[] rojos = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
        return Array.IndexOf(rojos, numero) >= 0 ? "Rojo" : "Negro";
    }

    private static string FormatearFicha(decimal valor)
    {
        if (valor >= 1000000m)
        {
            decimal millones = valor / 1000000m;
            return millones % 1 == 0 ? $"{millones:N0}M" : $"{millones:0.#}M";
        }
        if (valor >= 1000m)
        {
            decimal miles = valor / 1000m;
            return miles % 1 == 0 ? $"{miles:N0}K" : $"{miles:0.#}K";
        }
        return valor.ToString("N0");
    }

    private static string MapearTipoApuesta(string tipo)
    {
        if (tipo == "Docena1" || tipo == "Docena2" || tipo == "Docena3") return "docena";
        return tipo.ToLower();
    }

    private void ActualizarSaldo()
    {
        SaldoTexto = $"Saldo: ${_usuario.Saldo:N2}";
    }
}
