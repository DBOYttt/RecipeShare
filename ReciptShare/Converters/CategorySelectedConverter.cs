using System.Collections.ObjectModel;
using System.Globalization;

namespace ReciptShare.Converters
{
    public class CategorySelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var category = value?.ToString();
            var selectedCategories = parameter as ObservableCollection<string>;

            if (selectedCategories?.Contains(category) == true)
            {
                return Color.FromArgb("#4CAF50"); // Selected color
            }
            return Color.FromArgb("#E0E0E0"); // Default color
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}