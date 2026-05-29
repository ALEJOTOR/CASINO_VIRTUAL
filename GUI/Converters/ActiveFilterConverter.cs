using System.Windows.Media;

namespace GUI.Converters;

public class ActiveFilterConverter : System.Windows.Data.IMultiValueConverter
{
    public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (values.Length >= 2 && values[0] is string filtroActivo && parameter is string btnValue)
        {
            if (filtroActivo == btnValue)
                return (SolidColorBrush)System.Windows.Application.Current.Resources["DoradoBrush"];
            else
                return (SolidColorBrush)System.Windows.Application.Current.Resources["BgHoverBrush"];
        }
        return (SolidColorBrush)System.Windows.Application.Current.Resources["BgHoverBrush"];
    }

    public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}
