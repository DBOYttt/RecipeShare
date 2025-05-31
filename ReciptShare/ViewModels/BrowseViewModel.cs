using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Models;
using ReciptShare.Services;
using System.Collections.ObjectModel;

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

        public BrowseViewModel()
        {
            Title = "Browse Recipes";
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var recipes = MockDataService.GetRecipes();
                AllRecipes = new ObservableCollection<Recipe>(recipes);
                
                // Get unique categories
                var allCategories = recipes
                    .SelectMany(r => r.Categories)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();
                
                allCategories.Insert(0, "All");
                Categories = new ObservableCollection<string>(allCategories);

                // Apply initial filtering
                ApplyFilters();
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", $"Failed to load recipes: {ex.Message}", "OK");
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            ApplyFilters();
        }

        partial void OnSelectedCategoryChanged(string value)
        {
            ApplyFilters();
        }

        partial void OnSortOptionChanged(string value)
        {
            ApplyFilters();
        }

        [RelayCommand]
        private void ApplyFilters()
        {
            try
            {
                var filtered = AllRecipes.AsEnumerable();

                // Apply category filter
                if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "All")
                {
                    filtered = filtered.Where(r => r.Categories.Contains(SelectedCategory));
                }

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    var searchLower = SearchText.ToLower();
                    filtered = filtered.Where(r => 
                        r.Title.ToLower().Contains(searchLower) ||
                        r.Description.ToLower().Contains(searchLower) ||
                        r.AuthorName.ToLower().Contains(searchLower) ||
                        r.Categories.Any(c => c.ToLower().Contains(searchLower)));
                }

                // Apply sorting
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
                await Task.Delay(1000);
                
                LoadData();
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
            }
        }
    }
}