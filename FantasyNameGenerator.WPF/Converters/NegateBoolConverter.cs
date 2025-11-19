using System.Globalization;
using System.Windows.Data;

namespace FantasyNameGenerator.WPF.Converters
{
    internal class NegateBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            throw new InvalidOperationException("The target must be a boolean");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            throw new InvalidOperationException("The target must be a boolean");
        }
    }
}
