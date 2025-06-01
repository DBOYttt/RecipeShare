using System.Collections;
using System.Globalization;

namespace ReciptShare.Converters
{
    public class ListIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList list && parameter != null)
            {
                var index = list.IndexOf(parameter);
                return (index + 1).ToString();
            }
            return "1";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}