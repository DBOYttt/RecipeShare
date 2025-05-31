using System.Globalization;

namespace ReciptShare.Converters
{
    public class CategoryToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var category = value?.ToString();
            var selectedCategory = parameter?.ToString();

            if (category == selectedCategory)
            {
                return Colors.White; // Selected text color
            }
            return Color.FromArgb("#333333"); // Default text color
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}