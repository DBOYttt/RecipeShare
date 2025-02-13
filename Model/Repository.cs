using System.Collections.ObjectModel;

namespace RecipeShare
{
  public static class Repository
  {
    public static ObservableCollection<Recipe> Recipes { get; set; } = new ObservableCollection<Recipe>();
  }
}
