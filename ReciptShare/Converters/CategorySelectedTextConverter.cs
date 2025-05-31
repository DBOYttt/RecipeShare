using System.Collections.ObjectModel;
using System.Globalization;

namespace ReciptShare.Converters
{
    public class CategorySelectedTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var category = value?.ToString();
            var selectedCategories = parameter as ObservableCollection<string>;

            if (selectedCategories?.Contains(category) == true)
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