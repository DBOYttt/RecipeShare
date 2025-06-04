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

        [ObservableProperty]
        bool isApiConnected;

        [ObservableProperty]
        string apiStatusMessage = string.Empty;

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
                if (_httpClientService?.IsApiAvailable == true)
                {
                    await LoadFromApiAsync();
                }
                else
                {
                    await LoadFromMockDataAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Error loading data: {ex.Message}");
                await LoadFromMockDataAsync();
                SetApiStatus(false);
                
                // Show user-friendly error
                await Shell.Current.DisplayAlert("Connection Issue", 
                    "Unable to load latest recipes. Showing offline content.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadFromApiAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[HomeViewModel] Loading data from API...");

                // Load current user profile (if authenticated)
                await LoadCurrentUserAsync();

                // Load featured recipes (highly rated or featured)
                await LoadFeaturedRecipesAsync();

                // Load popular/trending recipes (most recent by default)
                await LoadPopularRecipesAsync();

                SetApiStatus(true);
                System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Successfully loaded {FeaturedRecipes.Count} featured and {PopularRecipes.Count} popular recipes from API");
            }
            catch (ApiException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HomeViewModel] API Error: {ex.Message}");
                await LoadFromMockDataAsync();
                SetApiStatus(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Unexpected error: {ex.Message}");
                await LoadFromMockDataAsync();
                SetApiStatus(false);
            }
        }

        private async Task LoadCurrentUserAsync()
        {
            try
            {
                // Try to get current user profile if authenticated
                // For now, we'll use mock data since we need to implement authentication first
                CurrentUser = _mockDataService.GetCurrentUserInstance();
                System.Diagnostics.Debug.WriteLine("[HomeViewModel] Using mock user data (authentication not implemented yet)");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Error loading user profile: {ex.Message}");
                CurrentUser = _mockDataService.GetCurrentUserInstance();
            }
        }

        private async Task LoadFeaturedRecipesAsync()
        {
            try
            {
                // Load highest rated recipes as featured
                var response = await _httpClientService.GetAsync<RecipeListResponse>("/recipes?sort=rating&order=desc&limit=3");
                
                FeaturedRecipes.Clear();
                
                if (response?.Recipes?.Any() == true)
                {
                    foreach (var recipe in response.Recipes.Take(3))
                    {
                        // Sync API data with legacy properties
                        recipe.SyncFromApi();
                        FeaturedRecipes.Add(recipe);
                    }
                    System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Loaded {FeaturedRecipes.Count} featured recipes from API");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[HomeViewModel] No featured recipes found in API response");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Error loading featured recipes: {ex.Message}");
            }
        }

        private async Task LoadPopularRecipesAsync()
        {
            try
            {
                // Load recent recipes (most recent first) as popular
                var response = await _httpClientService.GetAsync<RecipeListResponse>("/recipes?sort=created_at&order=desc&limit=5");
                
                PopularRecipes.Clear();
                
                if (response?.Recipes?.Any() == true)
                {
                    foreach (var recipe in response.Recipes.Take(5))
                    {
                        // Sync API data with legacy properties
                        recipe.SyncFromApi();
                        PopularRecipes.Add(recipe);
                    }
                    System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Loaded {PopularRecipes.Count} popular recipes from API");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[HomeViewModel] No popular recipes found in API response");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Error loading popular recipes: {ex.Message}");
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

            CurrentUser = _mockDataService.GetCurrentUserInstance();
            System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Loaded {FeaturedRecipes.Count} featured and {PopularRecipes.Count} popular recipes from mock data");
        }

        private void SetApiStatus(bool isConnected)
        {
            IsApiConnected = isConnected;
            ApiStatusMessage = isConnected ? "Connected to RecipeShare API" : "Offline Mode - Using Local Data";
            
            // Trigger property change notifications
            OnPropertyChanged(nameof(IsApiConnected));
            OnPropertyChanged(nameof(ApiStatusMessage));
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
            // Use the ApiId if available, otherwise fall back to regular Id
            var recipeId = !string.IsNullOrEmpty(recipe.ApiId) ? recipe.ApiId : recipe.Id.ToString();
            await Shell.Current.GoToAsync($"recipedetail?recipeId={recipeId}");
        }

        [RelayCommand]
        private async Task ToggleFavorite(Recipe recipe)
        {
            if (recipe == null) return;
            
            try
            {
                if (_httpClientService?.IsApiAvailable == true && !string.IsNullOrEmpty(recipe.ApiId))
                {
                    // Call API to like/unlike recipe
                    var response = await _httpClientService.PostAsync<object>($"/recipes/{recipe.ApiId}/like");
                    
                    if (response != null)
                    {
                        // Toggle the local state
                        recipe.IsFavorited = !recipe.IsFavorited;
                        recipe.IsLikedByUser = recipe.IsFavorited;
                        
                        // Update likes count
                        if (recipe.IsFavorited)
                        {
                            recipe.LikesCount++;
                            System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Added '{recipe.Title}' to favorites via API");
                            await Shell.Current.DisplayAlert("Added to Favorites", 
                                $"'{recipe.Title}' has been added to your favorites! ❤️", "Great!");
                        }
                        else
                        {
                            recipe.LikesCount = Math.Max(0, recipe.LikesCount - 1);
                            System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Removed '{recipe.Title}' from favorites via API");
                        }
                    }
                }
                else
                {
                    // Fallback to local toggle for offline mode
                    recipe.IsFavorited = !recipe.IsFavorited;
                    
                    if (recipe.IsFavorited)
                    {
                        recipe.LikesCount++;
                        System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Added '{recipe.Title}' to favorites (offline)");
                        await Shell.Current.DisplayAlert("Added to Favorites", 
                            $"'{recipe.Title}' has been added to your favorites! ❤️\n(Will sync when online)", "Great!");
                    }
                    else
                    {
                        recipe.LikesCount = Math.Max(0, recipe.LikesCount - 1);
                        System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Removed '{recipe.Title}' from favorites (offline)");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HomeViewModel] Error toggling favorite: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Unable to update favorites. Please try again.", "OK");
            }
        }

        [RelayCommand]
        private async Task ViewAllFeatured()
        {
            System.Diagnostics.Debug.WriteLine("[HomeViewModel] Navigating to browse page for featured recipes");
            await Shell.Current.GoToAsync("//browse?filter=featured");
        }

        [RelayCommand]
        private async Task ViewAllPopular()
        {
            System.Diagnostics.Debug.WriteLine("[HomeViewModel] Navigating to browse page for popular recipes");
            await Shell.Current.GoToAsync("//browse?filter=popular");
        }

        [RelayCommand]
        private async Task AddNewRecipe()
        {
            System.Diagnostics.Debug.WriteLine("[HomeViewModel] Navigating to add recipe page");
            await Shell.Current.GoToAsync("addrecipe");
        }
    }
}