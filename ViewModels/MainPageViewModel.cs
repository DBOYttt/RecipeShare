using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace RecipeShare.ViewModels
{
  public class MainPageViewModel : INotifyPropertyChanged
  {
    private Recipe selectedRecipe;

    public ObservableCollection<Recipe> Recipes { get; set; }
    public ICommand ToggleFavoriteCommand { get; }

    public Recipe SelectedRecipe
    {
      get => selectedRecipe;
      set
      {
        if (selectedRecipe != value)
        {
          selectedRecipe = value;
          OnPropertyChanged();
          if (selectedRecipe != null)
          {
            OnRecipeSelected(selectedRecipe);
            selectedRecipe = null;
            OnPropertyChanged(nameof(SelectedRecipe));
          }
        }
      }
    }

    public MainPageViewModel()
    {
      Recipes = new ObservableCollection<Recipe>
            {
                new Recipe { Title = "Spaghetti Bolognese", Description = "Klasyczne włoskie spaghetti z sosem bolognese", IsFavorite = false },
                new Recipe { Title = "Sałatka Grecka", Description = "Świeża sałatka z fetą i oliwkami", IsFavorite = false }
            };

      foreach (var recipe in Recipes)
      {
        Repository.Recipes.Add(recipe);
      }

      ToggleFavoriteCommand = new Command<Recipe>(OnToggleFavorite);
    }

    private void OnToggleFavorite(Recipe recipe)
    {
      if (recipe == null)
        return;
      recipe.IsFavorite = !recipe.IsFavorite;
    }

    private async void OnRecipeSelected(Recipe selectedRecipe)
    {
      if (selectedRecipe == null)
        return;
      await Shell.Current.GoToAsync($"{nameof(RecipeDetailPage)}?title={selectedRecipe.Title}");
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}
