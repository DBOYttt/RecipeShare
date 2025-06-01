using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Models;
using ReciptShare.Services;

namespace ReciptShare.ViewModels
{
    public partial class EditProfileViewModel : BaseViewModel
    {
        [ObservableProperty]
        User currentUser;

        [ObservableProperty]
        string firstName = string.Empty;

        [ObservableProperty]
        string lastName = string.Empty;

        [ObservableProperty]
        string username = string.Empty;

        [ObservableProperty]
        string email = string.Empty;

        [ObservableProperty]
        string bio = string.Empty;

        [ObservableProperty]
        string profileImageUrl = string.Empty;

        [ObservableProperty]
        string location = string.Empty;

        [ObservableProperty]
        string website = string.Empty;

        [ObservableProperty]
        bool isSaving;

        [ObservableProperty]
        bool isPublicProfile = true;

        [ObservableProperty]
        bool allowRecipeNotifications = true;

        [ObservableProperty]
        bool allowFollowerNotifications = true;

        [ObservableProperty]
        bool allowCommentNotifications = true;

        public EditProfileViewModel()
        {
            Title = "Edit Profile";
            LoadUserData();
        }

        private void LoadUserData()
        {
            CurrentUser = MockDataService.GetCurrentUser();
            
            // Split full name into first and last name
            var nameParts = CurrentUser.FullName.Split(' ', 2);
            FirstName = nameParts[0];
            LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
            
            Username = CurrentUser.Username;
            Email = CurrentUser.Email;
            Bio = CurrentUser.Bio;
            ProfileImageUrl = CurrentUser.ProfileImageUrl;
            
            // These would come from user preferences in a real app
            Location = "London, UK"; // Example
            Website = ""; // Example
        }

        [RelayCommand]
        private async Task SaveProfile()
        {
            if (!ValidateProfile())
            {
                return;
            }

            IsSaving = true;

            try
            {
                // Update the current user object
                CurrentUser.FullName = $"{FirstName.Trim()} {LastName.Trim()}".Trim();
                CurrentUser.Username = Username.Trim();
                CurrentUser.Email = Email.Trim();
                CurrentUser.Bio = Bio.Trim();
                CurrentUser.ProfileImageUrl = ProfileImageUrl.Trim();

                // Simulate saving to backend
                await Task.Delay(1500);

                // In a real app, you would save to your backend here
                // await _userService.UpdateUserAsync(CurrentUser);

                await Shell.Current.DisplayAlert("Success!", 
                    "Your profile has been updated successfully! 🎉", "Great!");

                // Navigate back to profile
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to save profile: {ex.Message}", "OK");
            }
            finally
            {
                IsSaving = false;
            }
        }

        private bool ValidateProfile()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                Shell.Current.DisplayAlert("Validation Error", "Please enter your first name.", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                Shell.Current.DisplayAlert("Validation Error", "Please enter a username.", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                Shell.Current.DisplayAlert("Validation Error", "Please enter your email address.", "OK");
                return false;
            }

            if (!IsValidEmail(Email))
            {
                Shell.Current.DisplayAlert("Validation Error", "Please enter a valid email address.", "OK");
                return false;
            }

            if (Bio.Length > 500)
            {
                Shell.Current.DisplayAlert("Validation Error", "Bio must be 500 characters or less.", "OK");
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [RelayCommand]
        private async Task ChangeProfilePicture()
        {
            var action = await Shell.Current.DisplayActionSheet(
                "Change Profile Picture",
                "Cancel",
                null,
                "Take Photo",
                "Choose from Gallery",
                "Enter URL",
                "Remove Picture");

            switch (action)
            {
                case "Take Photo":
                    await Shell.Current.DisplayAlert("Camera", "Camera functionality will be available in a future update!", "OK");
                    break;
                case "Choose from Gallery":
                    await Shell.Current.DisplayAlert("Gallery", "Gallery selection will be available in a future update!", "OK");
                    break;
                case "Enter URL":
                    var newUrl = await Shell.Current.DisplayPromptAsync(
                        "Profile Picture URL",
                        "Enter the URL of your profile picture:",
                        initialValue: ProfileImageUrl);
                    if (!string.IsNullOrEmpty(newUrl))
                    {
                        ProfileImageUrl = newUrl;
                    }
                    break;
                case "Remove Picture":
                    ProfileImageUrl = string.Empty;
                    break;
            }
        }

        [RelayCommand]
        private async Task ResetForm()
        {
            var result = await Shell.Current.DisplayAlert("Reset Changes", 
                "Are you sure you want to reset all changes?", "Reset", "Cancel");

            if (result)
            {
                LoadUserData();
                await Shell.Current.DisplayAlert("Reset", "All changes have been reset.", "OK");
            }
        }

        [RelayCommand]
        private async Task GoBack()
        {
            var hasChanges = HasUnsavedChanges();

            if (hasChanges)
            {
                var result = await Shell.Current.DisplayAlert("Unsaved Changes", 
                    "You have unsaved changes. What would you like to do?", 
                    "Save & Exit", "Discard Changes");

                if (result)
                {
                    await SaveProfile();
                }
                else
                {
                    await Shell.Current.GoToAsync("..");
                }
            }
            else
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        private bool HasUnsavedChanges()
        {
            var currentFullName = $"{FirstName.Trim()} {LastName.Trim()}".Trim();
            return currentFullName != CurrentUser.FullName ||
                   Username.Trim() != CurrentUser.Username ||
                   Email.Trim() != CurrentUser.Email ||
                   Bio.Trim() != CurrentUser.Bio ||
                   ProfileImageUrl.Trim() != CurrentUser.ProfileImageUrl;
        }

        [RelayCommand]
        private async Task ShowPrivacyInfo()
        {
            await Shell.Current.DisplayAlert("Profile Privacy", 
                "🔒 Privacy Settings:\n\n" +
                "• Public Profile: Your profile will be visible to all users\n" +
                "• Private Profile: Only approved followers can see your recipes\n\n" +
                "📧 Notification Settings:\n\n" +
                "• Recipe Notifications: Get notified when someone likes or comments on your recipes\n" +
                "• Follower Notifications: Get notified when someone follows you\n" +
                "• Comment Notifications: Get notified about new comments", "Got it");
        }
    }
}