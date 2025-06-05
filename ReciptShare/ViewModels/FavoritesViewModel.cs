using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Models;
using ReciptShare.Services;
using System.Collections.ObjectModel;
using System.Linq;

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

        [ObservableProperty]
        string apiStatusMessage = string.Empty;

        private readonly MockDataService _mockDataService;
        private readonly IHttpClientService? _httpClientService;

        public FavoritesViewModel(MockDataService mockDataService, IHttpClientService httpClientService)
        {
            Title = "Favorites";
            _mockDataService = mockDataService;
            _httpClientService = httpClientService;

            CurrentUser = _mockDataService.GetCurrentUserInstance();
            FavoriteRecipes = new ObservableCollection<Recipe>();

            _ = LoadFavoritesAsync();
        }

        public FavoritesViewModel() : this(new MockDataService(), new HttpClientService())
        {
        }

        public async Task LoadFavoritesAsync()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                if (_httpClientService != null)
                {
                    await _httpClientService.CheckApiHealthAsync();
                    SetApiStatus(_httpClientService.IsApiAvailable);
                }

                if (_httpClientService?.IsApiAvailable == true)
                {
                    await LoadFavoritesFromApiAsync();
                }
                else
                {
                    LoadFavoritesFromMock();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[FavoritesViewModel] Error loading favorites: {ex.Message}");
                LoadFavoritesFromMock();
                SetApiStatus(false);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadFavoritesFromApiAsync()
        {
            try
            {
                var response = await _httpClientService!.GetAsync<RecipeListResponse>("/collections/favorites");

                var list = new List<Recipe>();

                if (response?.Recipes?.Any() == true)
                {
                    foreach (var recipe in response.Recipes)
                    {
                        recipe.SyncFromApi();
                        list.Add(recipe);
                    }
                }

                list = SortFavorites(list);

                FavoriteRecipes = new ObservableCollection<Recipe>(list);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[FavoritesViewModel] API error: {ex.Message}");
                LoadFavoritesFromMock();
                SetApiStatus(false);
            }
        }

        private void LoadFavoritesFromMock()
        {
            var allRecipes = _mockDataService.GetRecipesInstance();
            var favorites = allRecipes.Where(r => r.IsFavorited).ToList();

            favorites = SortFavorites(favorites);

            FavoriteRecipes = new ObservableCollection<Recipe>(favorites);
        }

        private List<Recipe> SortFavorites(List<Recipe> favorites)
        {
            return SortOption switch
            {
                "Latest" => favorites.OrderByDescending(r => r.CreatedDate).ToList(),
                "Rating" => favorites.OrderByDescending(r => r.Rating).ToList(),
                "Cooking Time" => favorites.OrderBy(r => r.TotalTimeMinutes).ToList(),
                "Alphabetical" => favorites.OrderBy(r => r.Title).ToList(),
                _ => favorites.OrderByDescending(r => r.CreatedDate).ToList()
            };
        }

        partial void OnSortOptionChanged(string value)
        {
            _ = LoadFavoritesAsync();
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
                await Task.Delay(500);
                await LoadFavoritesAsync();
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
                await LoadFavoritesAsync();
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