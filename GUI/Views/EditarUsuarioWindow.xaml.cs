using System.Windows;
using GUI.ViewModels;

namespace GUI.Views;

public partial class EditarUsuarioWindow : Window
{
    public EditarUsuarioWindow(EditarUsuarioViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
