using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace RecipeShare.Converters
{
  public class FavoriteTextConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is bool isFavorite)
        return isFavorite ? "Usuń z ulubionych" : "Dodaj do ulubionych";
      return "Dodaj do ulubionych";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
