using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Models;
using ReciptShare.Services;
using System.Collections.ObjectModel;

namespace ReciptShare.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<Recipe> featuredRecipes;

        [ObservableProperty]
        ObservableCollection<Recipe> popularRecipes;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        User currentUser;

        private readonly MockDataService _mockDataService;
        private readonly IHttpClientService? _httpClientService;
        private readonly IAuthenticationService? _authService;

        // Constructor for dependency injection
        public HomeViewModel(MockDataService mockDataService, IHttpClientService httpClientService, IAuthenticationService authService)
        {
            Title = "Recipe Share";
            _mockDataService = mockDataService;
            _httpClientService = httpClientService;
            _authService = authService;
            
            FeaturedRecipes = new ObservableCollection<Recipe>();
            PopularRecipes = new ObservableCollection<Recipe>();
            CurrentUser = _mockDataService.GetCurrentUserInstance();
            
            _ = LoadDataAsync();
        }

        // Parameterless constructor for XAML/fallback
        public HomeViewModel() : this(new MockDataService(), new HttpClientService(), new AuthenticationService(new HttpClientService()))
        {
        }

        private async Task LoadDataAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            
            try
            {
                // Check API health first
                if (_httpClientService != null)
                {
                    await _httpClientService.CheckApiHealthAsync();
                    SetApiStatus(_httpClientService.IsApiAvailable);
                    
                    System.Diagnostics.Debug.WriteLine($"[HomeViewModel] API Status: {_httpClientService.IsApiAvailable}");
                }
                
                // Load data based on API availability
                await LoadFromApiOrMockAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Error loading data: {ex.Message}");
                await LoadFromMockDataAsync();
                SetApiStatus(false);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadFromApiOrMockAsync()
        {
            try
            {
                // Check if we have API access
                if (_httpClientService?.IsApiAvailable == true)
                {
                    // We'll implement API calls in the next step
                    // For now, use mock data even when API is available
                    System.Diagnostics.Debug.WriteLine("[HomeViewModel] API available, but not implemented yet. Using mock data.");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[HomeViewModel] API unavailable. Using mock data.");
                }
                
                await LoadFromMockDataAsync();
            }
            catch (ApiException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HomeViewModel] API Error: {ex.Message}");
                await LoadFromMockDataAsync();
                SetApiStatus(false);
            }
        }

        private async Task LoadFromMockDataAsync()
        {
            await Task.Delay(500); // Simulate loading
            
            var recipes = _mockDataService.GetRecipesInstance();
            
            FeaturedRecipes.Clear();
            PopularRecipes.Clear();
            
            // Featured recipes (first 3)
            foreach (var recipe in recipes.Take(3))
            {
                FeaturedRecipes.Add(recipe);
            }
            
            // Popular recipes (sorted by rating)
            foreach (var recipe in recipes.OrderByDescending(r => r.Rating).Take(5))
            {
                PopularRecipes.Add(recipe);
            }

            System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Loaded {FeaturedRecipes.Count} featured and {PopularRecipes.Count} popular recipes from mock data");
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            
            try
            {
                await LoadDataAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task GoToRecipeDetail(Recipe recipe)
        {
            if (recipe == null) return;
            
            System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Navigating to recipe detail: {recipe.Title}");
            await Shell.Current.GoToAsync($"recipedetail?recipeId={recipe.Id}");
        }

        [RelayCommand]
        private async Task ToggleFavorite(Recipe recipe)
        {
            if (recipe == null) return;
            
            recipe.IsFavorited = !recipe.IsFavorited;
            
            if (recipe.IsFavorited)
            {
                recipe.LikesCount++;
                System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Added '{recipe.Title}' to favorites");
                await Shell.Current.DisplayAlert("Added to Favorites", 
                    $"'{recipe.Title}' has been added to your favorites! ❤️", "Great!");
            }
            else
            {
                recipe.LikesCount = Math.Max(0, recipe.LikesCount - 1);
                System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Removed '{recipe.Title}' from favorites");
            }
        }

        [RelayCommand]
        private async Task ViewAllFeatured()
        {
            System.Diagnostics.Debug.WriteLine("[HomeViewModel] Navigating to browse page for featured recipes");
            await Shell.Current.GoToAsync("//browse");
        }

        [RelayCommand]
        private async Task ViewAllPopular()
        {
            System.Diagnostics.Debug.WriteLine("[HomeViewModel] Navigating to browse page for popular recipes");
            await Shell.Current.GoToAsync("//browse");
        }

        [RelayCommand]
        private async Task AddNewRecipe()
        {
            System.Diagnostics.Debug.WriteLine("[HomeViewModel] Navigating to add recipe page");
            await Shell.Current.GoToAsync("addrecipe");
        }
    }
}