using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using GUI.ViewModels;

namespace GUI.Views;

public partial class UcHistorial : UserControl
{
    public UcHistorial()
    {
        InitializeComponent();
    }

    private void OnItemMouseEnter(object sender, MouseEventArgs e)
    {
        if (sender is Border border)
        {
            var brushColor = ((System.Windows.Media.SolidColorBrush?)Application.Current.Resources["BgHoverBrush"])?.Color;
            if (brushColor.HasValue)
            {
                var animation = new ColorAnimation
                {
                    To = brushColor.Value,
                    Duration = TimeSpan.FromMilliseconds(150),
                    EasingFunction = new System.Windows.Media.Animation.QuadraticEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut }
                };

                var storyboard = new Storyboard();
                storyboard.Children.Add(animation);
                Storyboard.SetTarget(animation, border);
                Storyboard.SetTargetProperty(animation, new PropertyPath("Background.Color"));
                storyboard.Begin();
            }
        }
    }

    private void OnItemMouseLeave(object sender, MouseEventArgs e)
    {
        if (sender is Border border)
        {
            var brushColor = ((System.Windows.Media.SolidColorBrush?)Application.Current.Resources["BgElevatedBrush"])?.Color;
            if (brushColor.HasValue)
            {
                var animation = new ColorAnimation
                {
                    To = brushColor.Value,
                    Duration = TimeSpan.FromMilliseconds(150),
                    EasingFunction = new System.Windows.Media.Animation.QuadraticEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut }
                };

                var storyboard = new Storyboard();
                storyboard.Children.Add(animation);
                Storyboard.SetTarget(animation, border);
                Storyboard.SetTargetProperty(animation, new PropertyPath("Background.Color"));
                storyboard.Begin();
            }
        }
    }

    private void OnItemMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is HistorialViewModel vm && sender is Border border)
        {
            if (border.DataContext is ENTITY.MovimientoResumen movimiento)
            {
                vm.SeleccionarMovimientoCommand.Execute(movimiento);
            }
        }
    }
}
