using System.Globalization;

namespace ReciptShare.Converters
{
    public class CategoryToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var category = value?.ToString();
            var selectedCategory = parameter?.ToString();

            if (category == selectedCategory)
            {
                return Color.FromArgb("#2196F3"); // Selected color
            }
            return Color.FromArgb("#E0E0E0"); // Default color
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}