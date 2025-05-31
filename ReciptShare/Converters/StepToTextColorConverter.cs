using System.Globalization;

namespace ReciptShare.Converters
{
    public class StepToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var step = value?.ToString();
            var currentStep = parameter?.ToString();

            if (step == currentStep)
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