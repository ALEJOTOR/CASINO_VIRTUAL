namespace GUI.Converters;

public class TipoToIconConverter : System.Windows.Data.IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        string tipo = value?.ToString() ?? "";
        return tipo switch
        {
            "deposito" => "↓",
            "retiro" => "↑",
            "ganancia" => "★",
            "perdida" => "◆",
            _ => "•"
        };
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}
