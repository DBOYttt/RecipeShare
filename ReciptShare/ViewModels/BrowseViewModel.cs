using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Models;
using ReciptShare.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace ReciptShare.ViewModels
{
    public partial class BrowseViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<Recipe> allRecipes;

        [ObservableProperty]
        ObservableCollection<Recipe> filteredRecipes;

        [ObservableProperty]
        ObservableCollection<string> categories;

        [ObservableProperty]
        string selectedCategory = "All";

        [ObservableProperty]
        string searchText = string.Empty;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string sortOption = "Latest";

        public List<string> SortOptions { get; } = new List<string>
        {
            "Latest",
            "Popular",
            "Rating",
            "Cooking Time",
            "Alphabetical"
        };

        [ObservableProperty]
        string apiStatusMessage = string.Empty;

        private readonly MockDataService _mockDataService;
        private readonly IHttpClientService? _httpClientService;

        public BrowseViewModel(MockDataService mockDataService, IHttpClientService httpClientService)
        {
            Title = "Browse Recipes";
            _mockDataService = mockDataService;
            _httpClientService = httpClientService;

            AllRecipes = new ObservableCollection<Recipe>();
            FilteredRecipes = new ObservableCollection<Recipe>();
            Categories = new ObservableCollection<string>();

            _ = LoadDataAsync();
        }

        public BrowseViewModel() : this(new MockDataService(), new HttpClientService())
        {
        }

        private async Task LoadDataAsync()
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
                    await LoadFromApiAsync();
                }
                else
                {
                    LoadFromMock();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BrowseViewModel] Error loading data: {ex.Message}");
                LoadFromMock();
                SetApiStatus(false);
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
                var response = await _httpClientService!.GetAsync<RecipeListResponse>("/recipes?limit=50");

                AllRecipes.Clear();

                if (response?.Recipes?.Any() == true)
                {
                    foreach (var recipe in response.Recipes)
                    {
                        recipe.SyncFromApi();
                        AllRecipes.Add(recipe);
                    }

                    var allCategories = response.Recipes
                        .SelectMany(r => r.Categories)
                        .Distinct()
                        .OrderBy(c => c)
                        .ToList();

                    allCategories.Insert(0, "All");
                    Categories = new ObservableCollection<string>(allCategories);
                }

                await ApplyFiltersAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BrowseViewModel] API error: {ex.Message}");
                LoadFromMock();
                SetApiStatus(false);
            }
        }

        private void LoadFromMock()
        {
            var recipes = _mockDataService.GetRecipesInstance();
            AllRecipes = new ObservableCollection<Recipe>(recipes);

            var allCategories = recipes
                .SelectMany(r => r.Categories)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            allCategories.Insert(0, "All");
            Categories = new ObservableCollection<string>(allCategories);

            ApplyFiltersOffline();
        }

        partial void OnSearchTextChanged(string value)
        {
            _ = ApplyFiltersAsync();
        }

        partial void OnSelectedCategoryChanged(string value)
        {
            _ = ApplyFiltersAsync();
        }

        partial void OnSortOptionChanged(string value)
        {
            _ = ApplyFiltersAsync();
        }

        [RelayCommand]
        private async Task ApplyFiltersAsync()
        {
            try
            {
                if (_httpClientService?.IsApiAvailable == true)
                {
                    var query = HttpUtility.ParseQueryString(string.Empty);
                    query["limit"] = "50";

                    if (!string.IsNullOrWhiteSpace(SearchText))
                        query["search"] = SearchText;

                    if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "All")
                        query["category"] = SelectedCategory;

                    query["sort"] = SortOption switch
                    {
                        "Latest" => "created_at",
                        "Popular" => "likes",
                        "Rating" => "rating",
                        "Cooking Time" => "time",
                        "Alphabetical" => "title",
                        _ => "created_at"
                    };

                    var response = await _httpClientService.GetAsync<RecipeListResponse>($"/recipes?{query}");

                    var filteredList = new List<Recipe>();

                    if (response?.Recipes?.Any() == true)
                    {
                        foreach (var recipe in response.Recipes)
                        {
                            recipe.SyncFromApi();
                            filteredList.Add(recipe);
                        }
                    }

                    FilteredRecipes = new ObservableCollection<Recipe>(filteredList);
                }
                else
                {
                    ApplyFiltersOffline();
                }
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", $"Failed to filter recipes: {ex.Message}", "OK");
            }
        }

        private void ApplyFiltersOffline()
        {
            try
            {
                var filtered = AllRecipes.AsEnumerable();

                if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "All")
                {
                    filtered = filtered.Where(r => r.Categories.Contains(SelectedCategory));
                }

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    var searchLower = SearchText.ToLower();
                    filtered = filtered.Where(r =>
                        r.Title.ToLower().Contains(searchLower) ||
                        r.Description.ToLower().Contains(searchLower) ||
                        r.AuthorName.ToLower().Contains(searchLower) ||
                        r.Categories.Any(c => c.ToLower().Contains(searchLower)));
                }

                filtered = SortOption switch
                {
                    "Latest" => filtered.OrderByDescending(r => r.CreatedDate),
                    "Popular" => filtered.OrderByDescending(r => r.LikesCount),
                    "Rating" => filtered.OrderByDescending(r => r.Rating),
                    "Cooking Time" => filtered.OrderBy(r => r.TotalTimeMinutes),
                    "Alphabetical" => filtered.OrderBy(r => r.Title),
                    _ => filtered.OrderByDescending(r => r.CreatedDate)
                };

                FilteredRecipes = new ObservableCollection<Recipe>(filtered);
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", $"Failed to filter recipes: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task GoToRecipeDetail(Recipe recipe)
        {
            if (recipe == null) return;
            
            await Shell.Current.GoToAsync($"recipedetail?id={recipe.Id}");
        }

        [RelayCommand]
        private async Task ToggleFavorite(Recipe recipe)
        {
            if (recipe == null) return;
            
            recipe.IsFavorited = !recipe.IsFavorited;
            if (recipe.IsFavorited)
            {
                recipe.LikesCount++;
            }
            else
            {
                recipe.LikesCount--;
            }
        }

        [RelayCommand]
        private async Task RefreshRecipes()
        {
            IsRefreshing = true;
            
            try
            {
                // Simulate network delay
                await Task.Delay(500);

                await LoadDataAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task ClearSearch()
        {
            SearchText = string.Empty;
            await ApplyFiltersAsync();
        }

        [RelayCommand]
        private async Task ShowFilterOptions()
        {
            var action = await Shell.Current.DisplayActionSheet(
                "Sort by",
                "Cancel",
                null,
                SortOptions.ToArray());

            if (!string.IsNullOrEmpty(action) && action != "Cancel")
            {
                SortOption = action;
                await ApplyFiltersAsync();
            }
        }
    }
}