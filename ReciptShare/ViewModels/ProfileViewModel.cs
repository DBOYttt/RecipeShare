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

        [ObservableProperty]
        ObservableCollection<Recipe> recentRecipes;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string selectedTab = "Recipes";

        [ObservableProperty]
        int totalLikes;

        [ObservableProperty]
        int totalViews;

        [ObservableProperty]
        double averageRating;

        public List<string> TabOptions { get; } = new List<string> { "Recipes", "Stats", "Settings" };

        public ProfileViewModel()
        {
            Title = "Profile";
            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                CurrentUser = MockDataService.GetCurrentUser();
                var allRecipes = MockDataService.GetRecipes();
                var myRecipes = allRecipes.Where(r => r.AuthorId == CurrentUser.Id).ToList();
                
                UserRecipes = new ObservableCollection<Recipe>(myRecipes.OrderByDescending(r => r.CreatedDate));
                RecentRecipes = new ObservableCollection<Recipe>(myRecipes.Take(3));

                // Calculate statistics
                CalculateUserStats();
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", $"Failed to load user data: {ex.Message}", "OK");
            }
        }

        private void CalculateUserStats()
        {
            if (UserRecipes?.Any() == true)
            {
                TotalLikes = UserRecipes.Sum(r => r.LikesCount);
                TotalViews = UserRecipes.Sum(r => r.LikesCount * 5); // Simulated view count
                AverageRating = Math.Round(UserRecipes.Average(r => r.Rating), 1);
            }
            else
            {
                TotalLikes = 0;
                TotalViews = 0;
                AverageRating = 0;
            }
        }

        [RelayCommand]
        private async Task GoToRecipeDetail(Recipe recipe)
        {
            if (recipe == null) return;
            
            await Shell.Current.GoToAsync($"recipedetail?id={recipe.Id}");
        }

        [RelayCommand]
        private async Task EditProfile()
        {
            await Shell.Current.GoToAsync("editprofile");
        }

        [RelayCommand]
        private async Task AddNewRecipe()
        {
            await Shell.Current.GoToAsync("addrecipe");
        }

        [RelayCommand]
        private async Task ViewAllRecipes()
        {
            await Shell.Current.GoToAsync("//browse");
        }

        [RelayCommand]
        private async Task RefreshProfile()
        {
            IsRefreshing = true;
            
            try
            {
                // Simulate network delay
                await Task.Delay(1000);
                
                LoadUserData();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task ShareProfile()
        {
            try
            {
                var shareText = $"Check out {CurrentUser.FullName}'s recipes on RecipeShare!\n\n" +
                               $"👨‍🍳 {CurrentUser.RecipesCount} recipes shared\n" +
                               $"⭐ {AverageRating:F1} average rating\n" +
                               $"❤️ {TotalLikes} total likes\n\n" +
                               $"Bio: {CurrentUser.Bio}";

                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = shareText,
                    Title = "Share Profile"
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to share profile: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task ShowSettings()
        {
            SelectedTab = "Settings";
        }

        [RelayCommand]
        private async Task ShowStats()
        {
            SelectedTab = "Stats";
        }

        [RelayCommand]
        private async Task ShowRecipes()
        {
            SelectedTab = "Recipes";
        }

        [RelayCommand]
        private async Task ManageNotifications()
        {
            await Shell.Current.DisplayAlert("Notifications", 
                "Notification settings:\n\n" +
                "✓ New followers\n" +
                "✓ Recipe likes\n" +
                "✓ Recipe comments\n" +
                "✗ Recipe recommendations\n\n" +
                "Settings will be customizable in a future update!", "OK");
        }

        [RelayCommand]
        private async Task ManagePrivacy()
        {
            await Shell.Current.DisplayAlert("Privacy Settings", 
                "Privacy settings:\n\n" +
                "Profile visibility: Public\n" +
                "Recipe visibility: Public\n" +
                "Show followers: Yes\n" +
                "Show following: Yes\n\n" +
                "Privacy controls will be available in a future update!", "OK");
        }

        [RelayCommand]
        private async Task ViewHelp()
        {
            await Shell.Current.DisplayAlert("Help & Support", 
                "Need help with RecipeShare?\n\n" +
                "📧 Email: support@recipeshare.com\n" +
                "📱 Phone: +1 (555) 123-4567\n" +
                "🌐 Website: www.recipeshare.com/help\n" +
                "💬 Chat: Available in app settings\n\n" +
                "We're here to help!", "OK");
        }

        [RelayCommand]
        private async Task ViewAbout()
        {
            await Shell.Current.DisplayAlert("About RecipeShare", 
                "RecipeShare v1.0.0\n\n" +
                "The ultimate recipe sharing platform for food lovers!\n\n" +
                "Created with ❤️ using .NET MAUI\n" +
                "© 2025 RecipeShare Team\n\n" +
                "Thank you for using RecipeShare!", "OK");
        }

        [RelayCommand]
        private async Task SignOut()
        {
            var result = await Shell.Current.DisplayAlert("Sign Out", 
                "Are you sure you want to sign out?", "Sign Out", "Cancel");

            if (result)
            {
                await Shell.Current.DisplayAlert("Signed Out", 
                    "You have been signed out successfully!\n\n" +
                    "Note: In a real app, this would navigate to a login screen.", "OK");
            }
        }

        [RelayCommand]
        private async Task DeleteAccount()
        {
            var result = await Shell.Current.DisplayAlert("Delete Account", 
                "⚠️ WARNING ⚠️\n\n" +
                "This will permanently delete your account and all your recipes. " +
                "This action cannot be undone.\n\n" +
                "Are you absolutely sure?", "Delete Forever", "Cancel");

            if (result)
            {
                var confirmResult = await Shell.Current.DisplayAlert("Final Confirmation", 
                    "Type 'DELETE' to confirm account deletion:", "Cancel", "I understand");

                if (!confirmResult)
                {
                    await Shell.Current.DisplayAlert("Account Deletion", 
                        "Account deletion cancelled. Your data is safe!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Account Deleted", 
                        "Your account has been scheduled for deletion.\n\n" +
                        "Note: In a real app, this would handle actual account deletion.", "OK");
                }
            }
        }
    }
}