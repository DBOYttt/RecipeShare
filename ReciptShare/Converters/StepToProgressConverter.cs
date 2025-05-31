using System.Globalization;

namespace ReciptShare.Converters
{
    public class StepToProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentStep = value?.ToString();
            var steps = new List<string> { "Basic Info", "Ingredients", "Instructions", "Categories", "Final Review" };
            
            var index = steps.IndexOf(currentStep);
            if (index >= 0)
            {
                return (double)(index + 1) / steps.Count;
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}