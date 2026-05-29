using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using GUI.Converters;
using GUI.ViewModels;

namespace GUI.Views;

public partial class UcMinas : UserControl
{
    private MinasViewModel? _vm;
    private static readonly StringToBrushConverter _colorConverter = new();

    public UcMinas()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is MinasViewModel vm)
        {
            _vm = vm;
            vm.PropertyChanged += OnViewModelPropertyChanged;
            RebuildBoard();
        }
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(MinasViewModel.Filas) or nameof(MinasViewModel.Columnas) or nameof(MinasViewModel.Celdas))
            RebuildBoard();
    }

    private void RebuildBoard()
    {
        if (_vm == null) return;

        int filas = _vm.Filas;
        int cols = _vm.Columnas;
        int total = filas * cols;

        // Guard: ensure Celdas has the right count and prevent redundant rebuilds
        if (_vm.Celdas.Count != total) return;
        if (BoardGrid.Children.Count == total) return;  // Already built, avoid rebuild

        BoardGrid.Children.Clear();
        BoardGrid.RowDefinitions.Clear();
        BoardGrid.ColumnDefinitions.Clear();

        double cellSize = 56;

        for (int i = 0; i < filas; i++)
            BoardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(cellSize) });
        for (int j = 0; j < cols; j++)
            BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(cellSize) });

        // Inherit from global Button style to use the custom ControlTemplate
        // that respects Background/Foreground bindings (vs. ButtonChrome native)
        var baseStyle = (Style)FindResource(typeof(Button));
        var style = new Style(typeof(Button), baseStyle);
        
        style.Setters.Add(new Setter(Button.FontSizeProperty, 20.0));
        style.Setters.Add(new Setter(Button.FontWeightProperty, FontWeights.Bold));
        style.Setters.Add(new Setter(Button.MarginProperty, new Thickness(1)));
        style.Setters.Add(new Setter(Button.CursorProperty, Cursors.Hand));
        style.Setters.Add(new Setter(Button.BorderBrushProperty, new SolidColorBrush(Color.FromRgb(47, 78, 98))));
        style.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(2)));

        var disabledTrigger = new Trigger { Property = UIElement.IsEnabledProperty, Value = false };
        disabledTrigger.Setters.Add(new Setter(UIElement.OpacityProperty, 0.6));
        style.Triggers.Add(disabledTrigger);

        for (int i = 0; i < total; i++)
        {
            CreateBoardCell(i, cols, style);
        }
    }

    private void CreateBoardCell(int idx, int cols, Style style)
    {
        var btn = new Button
        {
            Style = style
        };

        // Bindings ONLY - NO local values for Background/Foreground
        // Local values override bindings, so we must NOT set them here
        btn.SetBinding(Button.ContentProperty, new Binding($"Celdas[{idx}].Texto"));
        btn.SetBinding(Button.BackgroundProperty, new Binding($"Celdas[{idx}].ColorFondo") { Converter = _colorConverter });
        btn.SetBinding(Button.ForegroundProperty, new Binding($"Celdas[{idx}].ColorTexto") { Converter = _colorConverter });
        btn.SetBinding(Button.IsEnabledProperty, new Binding($"Celdas[{idx}].Habilitada"));
        btn.SetBinding(Button.CommandProperty, new Binding("DestaparCeldaCommand") { Source = _vm });
        btn.SetBinding(Button.CommandParameterProperty, new Binding($"Celdas[{idx}].Indice") { Source = _vm });

        // Grid positioning
        Grid.SetRow(btn, idx / cols);
        Grid.SetColumn(btn, idx % cols);
        BoardGrid.Children.Add(btn);
    }
}
