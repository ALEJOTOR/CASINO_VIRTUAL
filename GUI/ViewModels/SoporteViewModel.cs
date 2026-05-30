using BLL;
using ENTITY;
using GUI.Commands;

namespace GUI.ViewModels;

public class SoporteViewModel : ViewModelBase
{
    public const string BotUrl = "https://t.me/CasinoRoyalAsistenteBot";
    private readonly Usuario _usuario;
    private readonly TelegramVinculoServicio _telegramSvc = new();
    private string _codigoPendiente = "Sin codigo pendiente";
    private string _estadoTelegram = "Escribe /vincular usuario en el bot para crear una solicitud.";
    private bool _hayCodigoPendiente;
    private bool _vinculoActivo;

    public RelayCommand AbrirBotCommand { get; }
    public RelayCommand RefrescarVinculoCommand { get; }
    public RelayCommand ConfirmarVinculoCommand { get; }

    public string CodigoPendiente
    {
        get => _codigoPendiente;
        set => SetProperty(ref _codigoPendiente, value);
    }

    public string EstadoTelegram
    {
        get => _estadoTelegram;
        set => SetProperty(ref _estadoTelegram, value);
    }

    public bool HayCodigoPendiente
    {
        get => _hayCodigoPendiente;
        set => SetProperty(ref _hayCodigoPendiente, value);
    }

    public bool VinculoActivo
    {
        get => _vinculoActivo;
        set => SetProperty(ref _vinculoActivo, value);
    }

    public SoporteViewModel(Usuario usuario)
    {
        _usuario = usuario;
        AbrirBotCommand = new RelayCommand(_ => AbrirBot());
        RefrescarVinculoCommand = new RelayCommand(_ => CargarVinculoPendiente());
        ConfirmarVinculoCommand = new RelayCommand(_ => ConfirmarVinculo());
        CargarVinculoPendiente();
    }

    private static void AbrirBot()
    {
        try { System.Diagnostics.Process.Start(BotUrl); }
        catch { }
    }

    private void CargarVinculoPendiente()
    {
        try
        {
            VinculoActivo = _telegramSvc.ExisteVinculoActivo(_usuario.IdUsuario);
            if (VinculoActivo)
            {
                HayCodigoPendiente = false;
                CodigoPendiente = "Cuenta vinculada";
                EstadoTelegram = "Tu cuenta ya esta vinculada con Telegram. Puedes usar /saldo y /transacciones.";
                return;
            }

            TelegramVinculo pendiente = _telegramSvc.ObtenerPendientePorUsuario(_usuario.IdUsuario);
            if (pendiente == null)
            {
                HayCodigoPendiente = false;
                CodigoPendiente = "Sin codigo pendiente";
                EstadoTelegram = "Escribe /vincular " + _usuario.Username + " en Telegram y luego actualiza esta seccion.";
                return;
            }

            HayCodigoPendiente = true;
            CodigoPendiente = pendiente.Codigo;
            EstadoTelegram = "Solicitud recibida desde Telegram. Confirma este codigo para vincular tu cuenta.";
        }
        catch (System.Exception ex)
        {
            HayCodigoPendiente = false;
            CodigoPendiente = "No disponible";
            EstadoTelegram = "No se pudo consultar la vinculacion: " + ex.Message;
        }
    }

    private void ConfirmarVinculo()
    {
        if (!HayCodigoPendiente)
        {
            EstadoTelegram = "No hay solicitudes pendientes para confirmar.";
            return;
        }

        try
        {
            string resultado = _telegramSvc.ConfirmarVinculo(_usuario.IdUsuario, CodigoPendiente);
            EstadoTelegram = resultado == "Guardado correctamente."
                ? "Cuenta vinculada correctamente. Ya puedes usar comandos privados en Telegram."
                : resultado;
            CargarVinculoPendiente();
        }
        catch (System.Exception ex)
        {
            EstadoTelegram = "Error al confirmar vinculacion: " + ex.Message;
        }
    }
}
