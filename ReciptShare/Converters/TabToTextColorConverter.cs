using System.Globalization;

namespace ReciptShare.Converters
{
    public class TabToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedTab = value?.ToString();
            var tabName = parameter?.ToString();

            if (selectedTab == tabName)
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