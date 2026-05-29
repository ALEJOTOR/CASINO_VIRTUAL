using System.IO;
using System.Windows.Media;

namespace GUI.Services;

public class SoundService
{
    private readonly MediaPlayer _loopPlayer = new();
    private readonly MediaPlayer _effectPlayer = new();
    private bool _loopActivo = false;

    private static string RutaSonido(string archivo)
        => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Sounds", archivo);

    public void IniciarGiro()
    {
        try
        {
            _loopPlayer.Open(new Uri(RutaSonido("spin.wav"), UriKind.Absolute));
            _loopPlayer.MediaEnded += OnLoopTerminado;
            _loopActivo = true;
            _loopPlayer.Play();
        }
        catch { /* Si el archivo no existe, continúa sin sonido */ }
    }

    private void OnLoopTerminado(object? sender, EventArgs e)
    {
        if (_loopActivo)
        {
            _loopPlayer.Position = TimeSpan.Zero;
            _loopPlayer.Play();
        }
    }

    public void DetenerGiro()
    {
        _loopActivo = false;
        _loopPlayer.MediaEnded -= OnLoopTerminado;
        _loopPlayer.Stop();
    }

    public void ReproducirVictoria()
    {
        try
        {
            _effectPlayer.Open(new Uri(RutaSonido("win.wav"), UriKind.Absolute));
            _effectPlayer.Play();
        }
        catch { }
    }

    public void ReproducirDerrota()
    {
        try
        {
            _effectPlayer.Open(new Uri(RutaSonido("lose.wav"), UriKind.Absolute));
            _effectPlayer.Play();
        }
        catch { }
    }

    public void ReproducirJackpot()
    {
        try
        {
            _effectPlayer.Open(new Uri(RutaSonido("jackpot.wav"), UriKind.Absolute));
            _effectPlayer.Play();
        }
        catch { }
    }
}
