using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using GUI.ViewModels;

namespace GUI.Views;

public partial class UcTragamonedas : UserControl
{
    private Storyboard? _victoriaStoryboard;

    public UcTragamonedas()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is TragamonedasViewModel vm)
        {
            vm.PropertyChanged += OnViewModelPropertyChanged;
        }
        if (e.OldValue is TragamonedasViewModel oldVm)
        {
            oldVm.PropertyChanged -= OnViewModelPropertyChanged;
        }
    }

    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TragamonedasViewModel.HayGanancia))
        {
            var vm = (TragamonedasViewModel)sender!;
            if (vm.HayGanancia)
                IniciarAnimacionVictoria(vm.EsJackpot);
            else
                DetenerAnimacionVictoria();
        }
    }

    private void IniciarAnimacionVictoria(bool esJackpot)
    {
        DetenerAnimacionVictoria();

        Color colorBorde = esJackpot ? Color.FromRgb(250, 204, 21) : Color.FromRgb(34, 197, 94);
        double duracion = esJackpot ? 0.4 : 0.6;

        _victoriaStoryboard = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };

        foreach (var (border, scale) in new[]
        {
            (BorderRollo1, ScaleRollo1),
            (BorderRollo2, ScaleRollo2),
            (BorderRollo3, ScaleRollo3)
        })
        {
            // Pulso de escala
            var scaleAnim = new DoubleAnimationUsingKeyFrames();
            scaleAnim.KeyFrames.Add(new EasingDoubleKeyFrame(1.0, KeyTime.FromPercent(0)));
            scaleAnim.KeyFrames.Add(new EasingDoubleKeyFrame(1.05, KeyTime.FromPercent(0.5)));
            scaleAnim.KeyFrames.Add(new EasingDoubleKeyFrame(1.0, KeyTime.FromPercent(1)));
            scaleAnim.Duration = TimeSpan.FromSeconds(duracion);

            Storyboard.SetTarget(scaleAnim, border);
            Storyboard.SetTargetProperty(scaleAnim,
                new PropertyPath("RenderTransform.ScaleX"));
            _victoriaStoryboard.Children.Add(scaleAnim);

            var scaleAnimY = scaleAnim.Clone();
            Storyboard.SetTarget(scaleAnimY, border);
            Storyboard.SetTargetProperty(scaleAnimY,
                new PropertyPath("RenderTransform.ScaleY"));
            _victoriaStoryboard.Children.Add(scaleAnimY);

            // Parpadeo de borde: dorado → colorVictoria → dorado
            var borderAnim = new ColorAnimationUsingKeyFrames();
            borderAnim.KeyFrames.Add(new EasingColorKeyFrame(
                Color.FromRgb(251, 191, 36), KeyTime.FromPercent(0)));
            borderAnim.KeyFrames.Add(new EasingColorKeyFrame(
                colorBorde, KeyTime.FromPercent(0.5)));
            borderAnim.KeyFrames.Add(new EasingColorKeyFrame(
                Color.FromRgb(251, 191, 36), KeyTime.FromPercent(1)));
            borderAnim.Duration = TimeSpan.FromSeconds(duracion);

            Storyboard.SetTarget(borderAnim, border);
            Storyboard.SetTargetProperty(borderAnim,
                new PropertyPath("BorderBrush.Color"));
            _victoriaStoryboard.Children.Add(borderAnim);
        }

        _victoriaStoryboard.Begin();
    }

    private void DetenerAnimacionVictoria()
    {
        _victoriaStoryboard?.Stop();
        _victoriaStoryboard = null;

        // Resetear bordes a estado normal
        foreach (var border in new[] { BorderRollo1, BorderRollo2, BorderRollo3 })
        {
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(251, 191, 36));
        }
    }
}
