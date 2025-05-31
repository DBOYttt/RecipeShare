using System.Globalization;

namespace ReciptShare.Converters
{
    public class IsNotAllCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var category = value?.ToString();
            return !string.IsNullOrEmpty(category) && category != "All";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}