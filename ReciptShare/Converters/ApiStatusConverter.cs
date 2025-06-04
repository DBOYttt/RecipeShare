using System.Globalization;

namespace ReciptShare.Converters
{
        public class ApiStatusConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is bool isConnected)
                {
                    return isConnected ? "🟢 API Connected" : "🟠 Offline Mode";
                }
                return "⚫ Unknown Status";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    
    }