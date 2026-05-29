namespace GUI.Converters;

public class FechaToStringConverter : System.Windows.Data.IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy HH:mm:ss");
        }
        return "";
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}
