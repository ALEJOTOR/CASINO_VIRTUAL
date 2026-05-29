using System.Windows.Media;

namespace GUI.Converters;

public class TipoToColorConverter : System.Windows.Data.IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        string tipo = value?.ToString() ?? "";
        return tipo switch
        {
            "deposito" => (SolidColorBrush)System.Windows.Application.Current.Resources["VerdeBrush"],
            "retiro" => (SolidColorBrush)System.Windows.Application.Current.Resources["RojoBrush"],
            "ganancia" => (SolidColorBrush)System.Windows.Application.Current.Resources["DoradoBrush"],
            "perdida" => (SolidColorBrush)System.Windows.Application.Current.Resources["VioletaBrush"],
            _ => (SolidColorBrush)System.Windows.Application.Current.Resources["TextoPrimarioBrush"]
        };
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}
