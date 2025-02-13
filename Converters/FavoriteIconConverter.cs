using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace RecipeShare.Converters
{
  public class FavoriteIconConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is bool isFavorite)
      {
        return isFavorite ? "delete.png" : "favorite.png";
      }
      return "favorite.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
