using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Models;
using ReciptShare.Services;
using System.Collections.ObjectModel;

namespace ReciptShare.ViewModels
{
    [QueryProperty(nameof(RecipeId), "id")]
    public partial class ReciptDetailViewModel : BaseViewModel
    {
        [ObservableProperty]
        Recipe currentRecipe;

        [ObservableProperty]
        ObservableCollection<Comment> comments;

        [ObservableProperty]
        ObservableCollection<Rating> ratings;

        [ObservableProperty]
        ObservableCollection<NumberedInstruction> instructionsWithNumbers;

        [ObservableProperty]
        string newCommentText = string.Empty;

        [ObservableProperty]
        int userRating = 0;

        [ObservableProperty]
        string userReview = string.Empty;

        [ObservableProperty]
        bool isAddingToShoppingList;

        [ObservableProperty]
        User currentUser;

        [ObservableProperty]
        int recipeId;

        public ReciptDetailViewModel()
        {
            Title = "Recipe Details";
            CurrentUser = MockDataService.GetCurrentUser();
            Comments = new ObservableCollection<Comment>();
            Ratings = new ObservableCollection<Rating>();
            InstructionsWithNumbers = new ObservableCollection<NumberedInstruction>();
        }

        partial void OnRecipeIdChanged(int value)
        {
            LoadRecipeData(value);
        }

        private void LoadRecipeData(int id)
        {
            try
            {
                CurrentRecipe = MockDataService.GetRecipeById(id);
                if (CurrentRecipe != null)
                {
                    Title = CurrentRecipe.Title;
                    LoadComments();
                    LoadRatings();
                    LoadNumberedInstructions();
                }
            }
            catch (Exception ex)
            {
                // Log error or handle gracefully
                Shell.Current.DisplayAlert("Error", $"Failed to load recipe: {ex.Message}", "OK");
            }
        }

        private void LoadComments()
        {
            var allComments = MockDataService.GetComments();
            var recipeComments = allComments.Where(c => c.RecipeId == CurrentRecipe.Id).ToList();
            Comments.Clear();
            foreach (var comment in recipeComments)
            {
                Comments.Add(comment);
            }
        }

        private void LoadRatings()
        {
            var allRatings = MockDataService.GetRatings();
            var recipeRatings = allRatings.Where(r => r.RecipeId == CurrentRecipe.Id).ToList();
            Ratings.Clear();
            foreach (var rating in recipeRatings)
            {
                Ratings.Add(rating);
            }
        }

        private void LoadNumberedInstructions()
        {
            InstructionsWithNumbers.Clear();
            if (CurrentRecipe?.Instructions != null)
            {
                for (int i = 0; i < CurrentRecipe.Instructions.Count; i++)
                {
                    InstructionsWithNumbers.Add(new NumberedInstruction
                    {
                        Number = i + 1,
                        Instruction = CurrentRecipe.Instructions[i]
                    });
                }
            }
        }

        [RelayCommand]
        private async Task ToggleFavorite()
        {
            try
            {
                if (CurrentRecipe == null) return;

                CurrentRecipe.IsFavorited = !CurrentRecipe.IsFavorited;
                
                if (CurrentRecipe.IsFavorited)
                {
                    CurrentRecipe.LikesCount++;
                    await Shell.Current.DisplayAlert("Added to Favorites", $"{CurrentRecipe.Title} has been added to your favorites!", "OK");
                }
                else
                {
                    CurrentRecipe.LikesCount--;
                    await Shell.Current.DisplayAlert("Removed from Favorites", $"{CurrentRecipe.Title} has been removed from your favorites.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to toggle favorite: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task AddComment()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewCommentText) || CurrentRecipe == null) return;

                var newComment = new Comment
                {
                    Id = Comments.Count + 1,
                    RecipeId = CurrentRecipe.Id,
                    UserId = CurrentUser.Id,
                    UserName = CurrentUser.Username,
                    UserAvatarUrl = CurrentUser.ProfileImageUrl,
                    Content = NewCommentText,
                    CreatedDate = DateTime.Now,
                    LikesCount = 0
                };

                Comments.Insert(0, newComment);
                CurrentRecipe.CommentsCount++;
                NewCommentText = string.Empty;

                await Shell.Current.DisplayAlert("Comment Added", "Your comment has been posted!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to add comment: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task AddRating()
        {
            try
            {
                if (UserRating == 0 || CurrentRecipe == null) return;

                var newRating = new Rating
                {
                    Id = Ratings.Count + 1,
                    RecipeId = CurrentRecipe.Id,
                    UserId = CurrentUser.Id,
                    UserName = CurrentUser.Username,
                    Stars = UserRating,
                    Review = UserReview,
                    CreatedDate = DateTime.Now
                };

                Ratings.Insert(0, newRating);
                
                // Recalculate recipe rating
                var avgRating = Ratings.Average(r => r.Stars);
                CurrentRecipe.Rating = Math.Round(avgRating, 1);
                CurrentRecipe.RatingsCount = Ratings.Count;

                UserRating = 0;
                UserReview = string.Empty;

                await Shell.Current.DisplayAlert("Rating Added", "Thank you for your rating!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to add rating: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task AddAllIngredientsToShoppingList()
        {
            try
            {
                if (CurrentRecipe?.Ingredients == null) return;

                IsAddingToShoppingList = true;

                // Simulate adding to shopping list
                await Task.Delay(1000);

                foreach (var ingredient in CurrentRecipe.Ingredients)
                {
                    ingredient.IsSelected = true;
                }

                IsAddingToShoppingList = false;

                await Shell.Current.DisplayAlert("Added to Shopping List", 
                    $"All ingredients from {CurrentRecipe.Title} have been added to your shopping list!", "OK");
            }
            catch (Exception ex)
            {
                IsAddingToShoppingList = false;
                await Shell.Current.DisplayAlert("Error", $"Failed to add to shopping list: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task ShareRecipe()
        {
            try
            {
                if (CurrentRecipe == null) return;

                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = $"Check out this amazing recipe: {CurrentRecipe.Title} by {CurrentRecipe.AuthorName}",
                    Title = "Share Recipe"
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to share recipe: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task LikeComment(Comment comment)
        {
            try
            {
                if (comment == null) return;

                comment.IsLiked = !comment.IsLiked;
                if (comment.IsLiked)
                {
                    comment.LikesCount++;
                }
                else
                {
                    comment.LikesCount--;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to like comment: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task ViewAuthorProfile()
        {
            try
            {
                if (CurrentRecipe == null) return;

                await Shell.Current.DisplayAlert("Author Profile", 
                    $"Viewing profile for {CurrentRecipe.AuthorName} - This feature will be implemented soon!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to view profile: {ex.Message}", "OK");
            }
        }
    }
}