using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Models;
using ReciptShare.Services;
using System.Collections.ObjectModel;

namespace ReciptShare.ViewModels
{
    public partial class FavoritesViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<Recipe> favoriteRecipes;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        User currentUser;

        [ObservableProperty]
        string sortOption = "Latest";

        public List<string> SortOptions { get; } = new List<string> 
        { 
            "Latest", 
            "Rating", 
            "Cooking Time", 
            "Alphabetical" 
        };

        public FavoritesViewModel()
        {
            Title = "Favorites";
            CurrentUser = MockDataService.GetCurrentUser();
            LoadFavorites();
        }

        public void LoadFavorites()
        {
            try
            {
                var allRecipes = MockDataService.GetRecipes();
                var favorites = allRecipes.Where(r => r.IsFavorited).ToList();
                
                // Apply sorting
                var sorted = SortOption switch
                {
                    "Latest" => favorites.OrderByDescending(r => r.CreatedDate),
                    "Rating" => favorites.OrderByDescending(r => r.Rating),
                    "Cooking Time" => favorites.OrderBy(r => r.TotalTimeMinutes),
                    "Alphabetical" => favorites.OrderBy(r => r.Title),
                    _ => favorites.OrderByDescending(r => r.CreatedDate)
                };

                FavoriteRecipes = new ObservableCollection<Recipe>(sorted);
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", $"Failed to load favorites: {ex.Message}", "OK");
            }
        }

        partial void OnSortOptionChanged(string value)
        {
            LoadFavorites();
        }

        [RelayCommand]
        private async Task GoToRecipeDetail(Recipe recipe)
        {
            if (recipe == null) return;
            
            await Shell.Current.GoToAsync($"recipedetail?id={recipe.Id}");
        }

        [RelayCommand]
        private async Task RemoveFromFavorites(Recipe recipe)
        {
            if (recipe == null) return;

            var result = await Shell.Current.DisplayAlert(
                "Remove Favorite", 
                $"Remove '{recipe.Title}' from your favorites?", 
                "Remove", 
                "Cancel");

            if (result)
            {
                recipe.IsFavorited = false;
                recipe.LikesCount--;
                FavoriteRecipes.Remove(recipe);
                
                await Shell.Current.DisplayAlert("Removed", "Recipe removed from favorites!", "OK");
            }
        }

        [RelayCommand]
        private async Task RefreshFavorites()
        {
            IsRefreshing = true;
            
            try
            {
                // Simulate network delay
                await Task.Delay(1000);
                
                LoadFavorites();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task ShowSortOptions()
        {
            var action = await Shell.Current.DisplayActionSheet(
                "Sort favorites by", 
                "Cancel", 
                null, 
                SortOptions.ToArray());

            if (!string.IsNullOrEmpty(action) && action != "Cancel")
            {
                SortOption = action;
            }
        }

        [RelayCommand]
        private async Task ClearAllFavorites()
        {
            if (!FavoriteRecipes.Any()) return;

            var result = await Shell.Current.DisplayAlert(
                "Clear All Favorites", 
                "Are you sure you want to remove all recipes from your favorites? This action cannot be undone.", 
                "Clear All", 
                "Cancel");

            if (result)
            {
                foreach (var recipe in FavoriteRecipes.ToList())
                {
                    recipe.IsFavorited = false;
                    recipe.LikesCount--;
                }
                
                FavoriteRecipes.Clear();
                await Shell.Current.DisplayAlert("Cleared", "All favorites have been removed!", "OK");
            }
        }

        [RelayCommand]
        private async Task BrowseRecipes()
        {
            await Shell.Current.GoToAsync("//browse");
        }
    }
}