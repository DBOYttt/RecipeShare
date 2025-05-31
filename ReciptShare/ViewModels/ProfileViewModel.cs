using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Models;
using ReciptShare.Services;
using System.Collections.ObjectModel;

namespace ReciptShare.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        [ObservableProperty]
        User currentUser;

        [ObservableProperty]
        ObservableCollection<Recipe> userRecipes;

        public ProfileViewModel()
        {
            Title = "Profile";
            LoadData();
        }

        private void LoadData()
        {
            CurrentUser = MockDataService.GetCurrentUser();
            var allRecipes = MockDataService.GetRecipes();
            var myRecipes = allRecipes.Where(r => r.AuthorId == CurrentUser.Id).ToList();
            UserRecipes = new ObservableCollection<Recipe>(myRecipes);
        }

        [RelayCommand]
        private async Task EditProfile()
        {
            await Shell.Current.GoToAsync("editprofile");
        }

        [RelayCommand]
        private async Task ViewSettings()
        {
            await Shell.Current.DisplayAlert("Settings", "Settings page coming soon!", "OK");
        }

        [RelayCommand]
        private async Task GoToRecipeDetail(Recipe recipe)
        {
            if (recipe == null) return;
            
            await Shell.Current.GoToAsync($"recipedetail?id={recipe.Id}");
        }
    }
}