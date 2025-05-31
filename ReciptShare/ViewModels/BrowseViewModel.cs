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
        ObservableCollection<Recipe> recipes;

        [ObservableProperty]
        ObservableCollection<Recipe> filteredRecipes;

        [ObservableProperty]
        ObservableCollection<string> categories;

        [ObservableProperty]
        string selectedCategory = "All";

        [ObservableProperty]
        string searchText = string.Empty;

        public BrowseViewModel()
        {
            Title = "Browse";
            LoadData();
        }

        private void LoadData()
        {
            var allRecipes = MockDataService.GetRecipes();
            Recipes = new ObservableCollection<Recipe>(allRecipes);
            FilteredRecipes = new ObservableCollection<Recipe>(allRecipes);

            var allCategories = allRecipes
                .SelectMany(r => r.Categories)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
            
            allCategories.Insert(0, "All");
            Categories = new ObservableCollection<string>(allCategories);
        }

        [RelayCommand]
        private void FilterByCategory(string category)
        {
            SelectedCategory = category;
            ApplyFilters();
        }

        [RelayCommand]
        private void SearchRecipes()
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filtered = Recipes.AsEnumerable();

            // Apply category filter
            if (SelectedCategory != "All")
            {
                filtered = filtered.Where(r => r.Categories.Contains(SelectedCategory));
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(r => 
                    r.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.AuthorName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            FilteredRecipes.Clear();
            foreach (var recipe in filtered)
            {
                FilteredRecipes.Add(recipe);
            }
        }

        [RelayCommand]
        private async Task GoToRecipeDetail(Recipe recipe)
        {
            if (recipe == null) return;
            
            await Shell.Current.GoToAsync($"recipedetail?id={recipe.Id}");
        }
    }
}