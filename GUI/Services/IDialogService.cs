namespace GUI.Services;

public interface IDialogService
{
    void ShowMessage(string message, string title = "Aviso");
    bool ShowConfirmation(string message, string title = "Confirmar");
    bool? ShowDialog(System.Windows.Window dialog);
}
