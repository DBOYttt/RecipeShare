using System.Globalization;

namespace ReciptShare.Converters
{
    public class IsNotLastStepConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentStep = value?.ToString();
            return currentStep != "Final Review";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}