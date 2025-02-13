using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace RecipeShare.ViewModels
{
  public class FavoritesViewModel : INotifyPropertyChanged
  {
    public ICommand RemoveFavoriteCommand { get; }

    private ObservableCollection<Recipe> favoriteRecipes;
    public ObservableCollection<Recipe> FavoriteRecipes
    {
      get => favoriteRecipes;
      set
      {
        if (favoriteRecipes != value)
        {
          favoriteRecipes = value;
          OnPropertyChanged();
        }
      }
    }

    public FavoritesViewModel()
    {

      RemoveFavoriteCommand = new Command<Recipe>(OnRemoveFavorite);


      FavoriteRecipes = new ObservableCollection<Recipe>(Repository.Recipes.Where(r => r.IsFavorite));

      foreach (var recipe in Repository.Recipes)
      {
        recipe.PropertyChanged += Recipe_PropertyChanged;
      }
      Repository.Recipes.CollectionChanged += Recipes_CollectionChanged;
    }

    private void OnRemoveFavorite(Recipe recipe)
    {
      if (recipe == null)
        return;
      recipe.IsFavorite = false;
    }

    private void Recipes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.NewItems != null)
      {
        foreach (Recipe newRecipe in e.NewItems)
        {
          newRecipe.PropertyChanged += Recipe_PropertyChanged;
        }
      }
      if (e.OldItems != null)
      {
        foreach (Recipe oldRecipe in e.OldItems)
        {
          oldRecipe.PropertyChanged -= Recipe_PropertyChanged;
        }
      }
      UpdateFavorites();
    }

    private void Recipe_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(Recipe.IsFavorite))
      {
        UpdateFavorites();
      }
    }

    private void UpdateFavorites()
    {
      var favorites = Repository.Recipes.Where(r => r.IsFavorite).ToList();
      FavoriteRecipes.Clear();
      foreach (var recipe in favorites)
      {
        FavoriteRecipes.Add(recipe);
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}
