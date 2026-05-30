using System.Windows;
using BLL;
using ENTITY;
using GUI.Commands;
using GUI.Services;

namespace GUI.ViewModels;

public class TragamonedasViewModel : ViewModelBase
{
    private readonly Usuario _usuario;
    private readonly PartidaServicio _servicio = new();
    private readonly UsuarioServicio _usuarioSvc = new();
    private readonly Random _random = new();
    private readonly SoundService _sound = new();
    private readonly string[] _simbolos = { "7️⃣", "💰", "🎰", "💎", "🍒" };

    private string _rollo1 = "7️⃣";
    private string _rollo2 = "7️⃣";
    private string _rollo3 = "7️⃣";
    private string _saldoTexto = "";
    private string _resultadoTexto = "Listo para girar";
    private string _jackpotTexto = "JACKPOT ROYAL";
    private string _apuestaTexto = "1000";
    private bool _girando;
    private bool _hayGanancia;
    private bool _esJackpot;

    // Estadísticas de sesión
    private int _jugadasSesion = 0;
    private int _rachaSesion = 0;
    private decimal _mayorGananciaSesion = 0;

    private string _jugadasTexto = "0";
    private string _rachaTexto = "0";
    private string _mayorGananciaTexto = "$0";

    public string Rollo1 { get => _rollo1; set => SetProperty(ref _rollo1, value); }
    public string Rollo2 { get => _rollo2; set => SetProperty(ref _rollo2, value); }
    public string Rollo3 { get => _rollo3; set => SetProperty(ref _rollo3, value); }
    public string SaldoTexto { get => _saldoTexto; set => SetProperty(ref _saldoTexto, value); }
    public string ResultadoTexto { get => _resultadoTexto; set => SetProperty(ref _resultadoTexto, value); }
    public string JackpotTexto { get => _jackpotTexto; set => SetProperty(ref _jackpotTexto, value); }
    public string ApuestaTexto { get => _apuestaTexto; set => SetProperty(ref _apuestaTexto, value); }
    public bool Girando { get => _girando; set => SetProperty(ref _girando, value); }
    public bool HayGanancia { get => _hayGanancia; set => SetProperty(ref _hayGanancia, value); }
    public bool EsJackpot { get => _esJackpot; set => SetProperty(ref _esJackpot, value); }
    public string JugadasTexto { get => _jugadasTexto; set => SetProperty(ref _jugadasTexto, value); }
    public string RachaTexto { get => _rachaTexto; set => SetProperty(ref _rachaTexto, value); }
    public string MayorGananciaTexto { get => _mayorGananciaTexto; set => SetProperty(ref _mayorGananciaTexto, value); }

    public RelayCommand GirarCommand { get; }
    public RelayCommand ApuestaRapidaCommand { get; }

    public event EventHandler? SaldoActualizado;

    public TragamonedasViewModel(Usuario usuario)
    {
        _usuario = usuario;
        GirarCommand = new RelayCommand(_ => Girar());
        ApuestaRapidaCommand = new RelayCommand(ApuestaRapida);
        ActualizarSaldo();
    }

    private void ApuestaRapida(object? parameter)
    {
        if (parameter is decimal valor)
            ApuestaTexto = valor.ToString("N0");
        else if (parameter is string s && decimal.TryParse(s, out decimal d))
            ApuestaTexto = d.ToString("N0");
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

        if (!decimal.TryParse(ApuestaTexto, out decimal apuesta) || apuesta <= 0)
        {
            MessageBox.Show("Ingrese una apuesta válida.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (apuesta > _usuario.Saldo)
        {
            MessageBox.Show($"Saldo insuficiente. Tu saldo es ${_usuario.Saldo:N2}.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Girando = true;
        HayGanancia = false;
        EsJackpot = false;

        try
        {
            ResultadoTexto = "Girando...";
            _sound.IniciarGiro();

            // Fase 1: giro rápido (todos cambian igual de rápido)
            for (int i = 0; i < 10; i++)
            {
                MostrarSimbolos(
                    _simbolos[_random.Next(_simbolos.Length)],
                    _simbolos[_random.Next(_simbolos.Length)],
                    _simbolos[_random.Next(_simbolos.Length)]);
                await Task.Delay(60);
            }

            // Fase 2: el primer rodillo se detiene
            string s1 = _simbolos[_random.Next(_simbolos.Length)];
            for (int i = 0; i < 8; i++)
            {
                MostrarSimbolos(s1,
                    _simbolos[_random.Next(_simbolos.Length)],
                    _simbolos[_random.Next(_simbolos.Length)]);
                await Task.Delay(70 + i * 10);
            }

            // Fase 3: el segundo rodillo se detiene
            string s2 = _simbolos[_random.Next(_simbolos.Length)];
            for (int i = 0; i < 6; i++)
            {
                MostrarSimbolos(s1, s2,
                    _simbolos[_random.Next(_simbolos.Length)]);
                await Task.Delay(80 + i * 15);
            }

            // Resultado final
            string s3 = _simbolos[_random.Next(_simbolos.Length)];
            MostrarSimbolos(s1, s2, s3);
            _sound.DetenerGiro();

            decimal ganancia = CalcularGanancia(apuesta, s1, s2, s3);
            bool gano = ganancia > 0;
            int multiplicador = ObtenerMultiplicador(s1, s2, s3);

            // Reproducir sonido según resultado
            if (multiplicador == 8)
            {
                _sound.ReproducirJackpot();
                ResultadoTexto = $"¡Ganaste ${ganancia:N2}!";
            }
            else if (gano)
            {
                _sound.ReproducirVictoria();
                ResultadoTexto = $"¡Ganaste ${ganancia:N2}!";
            }
            else
            {
                _sound.ReproducirDerrota();
                ResultadoTexto = $"Perdiste ${apuesta:N2}";
            }

            JackpotTexto = ObtenerTextoResultado(multiplicador);
            HayGanancia = gano;
            EsJackpot = (multiplicador == 8);

            // Actualizar estadísticas
            _jugadasSesion++;
            if (gano)
            {
                _rachaSesion = _rachaSesion >= 0 ? _rachaSesion + 1 : 1;
                if (ganancia > _mayorGananciaSesion) _mayorGananciaSesion = ganancia;
            }
            else
            {
                _rachaSesion = _rachaSesion <= 0 ? _rachaSesion - 1 : -1;
            }

            JugadasTexto = _jugadasSesion.ToString();
            RachaTexto = _rachaSesion > 0 ? $"+{_rachaSesion} 🔥"
                       : _rachaSesion < 0 ? $"{_rachaSesion}"
                       : "0";
            MayorGananciaTexto = $"${_mayorGananciaSesion:N0}";

            Partida partida = new Partida
            {
                IdUsuario = _usuario.IdUsuario,
                IdJuego = _servicio.ObtenerIdJuegoPorNombre("Tragamonedas"),
                IdEstado = gano ? 2 : 3,
                Apuesta = apuesta,
                Ganancia = ganancia
            };

            if (partida.IdJuego == 0)
            {
                MessageBox.Show("No se encontró el juego Tragamonedas en la base de datos. Reinicia la aplicación para inicializarlo.",
                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var (resultado, _, bonusExtra) = _servicio.RegistrarPartida(partida);
            if (resultado != "Guardado correctamente.")
            {
                MessageBox.Show(resultado, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (bonusExtra > 0)
                MessageBox.Show($"¡Bono activo! Ganaste ${bonusExtra:N2} extra.", "¡Bono Aplicado!",
                    MessageBoxButton.OK, MessageBoxImage.Information);

            Usuario? actualizado = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
            if (actualizado != null) _usuario.Saldo = actualizado.Saldo;
            ActualizarSaldo();
            SaldoActualizado?.Invoke(this, EventArgs.Empty);
        }
        finally
        {
            _sound.DetenerGiro();
            Girando = false;
        }
    }

    private void MostrarSimbolos(string s1, string s2, string s3)
    {
        Rollo1 = s1;
        Rollo2 = s2;
        Rollo3 = s3;
    }

    private static decimal CalcularGanancia(decimal apuesta, string s1, string s2, string s3)
    {
        int multiplicador = ObtenerMultiplicador(s1, s2, s3);
        if (multiplicador > 0) return apuesta * multiplicador;
        return 0m;
    }

    private static int ObtenerMultiplicador(string s1, string s2, string s3)
    {
        if (s1 == s2 && s2 == s3) return 8;
        if (s1 == s2 || s1 == s3 || s2 == s3) return 2;
        return 0;
    }

    private static string ObtenerTextoResultado(int multiplicador)
    {
        if (multiplicador == 8) return "JACKPOT: 3 iguales x8";
        if (multiplicador == 2) return "Premio menor: 2 iguales x2";
        return "Sin combinación ganadora";
    }

    private void ActualizarSaldo()
    {
        SaldoTexto = $"Saldo: ${_usuario.Saldo:N2}";
    }
}
