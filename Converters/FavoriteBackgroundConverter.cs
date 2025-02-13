using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace RecipeShare.Converters
{
  public class FavoriteBackgroundConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is bool isFavorite)
      {
        return isFavorite ? Colors.Red : Colors.Green;
      }
      return Colors.Green;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
