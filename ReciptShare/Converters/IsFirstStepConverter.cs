using System.Globalization;

namespace ReciptShare.Converters
{
    public class IsFirstStepConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentStep = value?.ToString();
            return currentStep == "Basic Info";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}