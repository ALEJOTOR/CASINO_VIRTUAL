namespace GUI.Converters;

public class MontoToStateConverter : System.Windows.Data.IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is decimal monto)
        {
            if (monto > 0)
                return "Crédito (ganancia)";
            else if (monto < 0)
                return "Débito (apuesta/retiro)";
            else
                return "Neutral";
        }
        return "Neutral";
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}
