namespace GUI.Converters;

public class TipoToDisplayConverter : System.Windows.Data.IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        string tipo = value?.ToString() ?? "";
        return tipo switch
        {
            "deposito" => "DEPÓSITO",
            "retiro" => "RETIRO",
            "ganancia" => "GANANCIA",
            "perdida" => "PÉRDIDA",
            _ => tipo.ToUpper()
        };
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}
