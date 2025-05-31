using System.Globalization;

namespace ReciptShare.Converters
{
    public class BoolToHeartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFavorited)
            {
                return isFavorited ? "❤️" : "🤍";
            }
            return "🤍";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}