using System.Windows;
using ENTITY;

namespace GUI.Views;

public partial class MainWindow : Window
{
    public MainWindow(Usuario usuario)
    {
        InitializeComponent();
        DataContext = new ViewModels.MainViewModel(usuario);
    }
}
