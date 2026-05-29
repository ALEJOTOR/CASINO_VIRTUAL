using System.Windows;
using System.Windows.Controls;

namespace GUI.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModels.LoginViewModel vm)
            vm.Password = pwBox.Password;
    }
}
