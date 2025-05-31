using System.Globalization;

namespace ReciptShare.Converters
{
    public class StringEqualityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value?.ToString();
            var parameterValue = parameter?.ToString();

            return stringValue == parameterValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}