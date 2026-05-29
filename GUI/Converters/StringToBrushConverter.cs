using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI.Converters;

public class StringToBrushConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string hex && !string.IsNullOrEmpty(hex))
        {
            try
            {
                // Remove '#' if present
                if (hex.StartsWith("#"))
                    hex = hex.Substring(1);

                // Parse hex string to integer
                if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int rgb))
                {
                    byte r = (byte)((rgb >> 16) & 0xFF);
                    byte g = (byte)((rgb >> 8) & 0xFF);
                    byte b = (byte)(rgb & 0xFF);
                    return new SolidColorBrush(Color.FromRgb(r, g, b));
                }
            }
            catch
            {
                return Brushes.Transparent;
            }
        }
        return Brushes.Transparent;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

