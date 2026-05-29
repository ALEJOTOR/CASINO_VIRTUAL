using System.Windows;
using ENTITY;

namespace GUI.Views;

public partial class AdminWindow : Window
{
    public AdminWindow(Usuario admin)
    {
        InitializeComponent();
        DataContext = new ViewModels.AdminViewModel(admin);
    }
}
