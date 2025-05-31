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
        User currentUser;

        [ObservableProperty]
        ObservableCollection<Recipe> popularRecipes;

        [ObservableProperty]
        ObservableCollection<Recipe> latestRecipes;

        public HomeViewModel()
        {
            Title = "Home";
            LoadData();
        }

        private void LoadData()
        {
            CurrentUser = MockDataService.GetCurrentUser();
            PopularRecipes = new ObservableCollection<Recipe>(MockDataService.GetPopularRecipes());
            LatestRecipes = new ObservableCollection<Recipe>(MockDataService.GetLatestRecipes());
        }

        [RelayCommand]
        private async Task GoToRecipeDetail(Recipe recipe)
        {
            if (recipe == null) return;
            
            await Shell.Current.GoToAsync($"recipedetail?id={recipe.Id}");
        }

        [RelayCommand]
        private async Task AddToFavorites(Recipe recipe)
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
        private async Task AddNewRecipe()
        {
            await Shell.Current.GoToAsync("addrecipe");
        }

        [RelayCommand]
        private async Task RefreshData()
        {
            IsBusy = true;
            
            // Simulate network delay
            await Task.Delay(1000);
            
            LoadData();
            
            IsBusy = false;
        }
    }
}