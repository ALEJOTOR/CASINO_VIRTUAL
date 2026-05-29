using System.Windows.Media;

namespace GUI.Converters;

public class MontoToColorConverter : System.Windows.Data.IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is decimal monto)
        {
            if (monto >= 0)
                return (SolidColorBrush)System.Windows.Application.Current.Resources["VerdeBrush"];
            else
                return (SolidColorBrush)System.Windows.Application.Current.Resources["RojoBrush"];
        }
        return (SolidColorBrush)System.Windows.Application.Current.Resources["TextoPrimarioBrush"];
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}
