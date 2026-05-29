using System.Windows;

namespace GUI.Services;

public class DialogService : IDialogService
{
    public void ShowMessage(string message, string title = "Aviso")
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public bool ShowConfirmation(string message, string title = "Confirmar")
    {
        return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    public bool? ShowDialog(Window dialog)
    {
        return dialog.ShowDialog();
    }
}
