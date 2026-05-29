using GUI.Commands;

namespace GUI.ViewModels;

public class SoporteViewModel : ViewModelBase
{
    public const string BotUrl = "https://t.me/CasinoRoyalAsistenteBot";

    public RelayCommand AbrirBotCommand { get; }

    public SoporteViewModel()
    {
        AbrirBotCommand = new RelayCommand(_ => AbrirBot());
    }

    private static void AbrirBot()
    {
        try { System.Diagnostics.Process.Start(BotUrl); }
        catch { }
    }
}
