using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Models;
using ReciptShare.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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

        private readonly MockDataService _mockDataService;
        private readonly IHttpClientService? _httpClientService;

        public ReciptDetailViewModel(MockDataService mockDataService, IHttpClientService httpClientService)
        {
            Title = "Recipe Details";
            _mockDataService = mockDataService;
            _httpClientService = httpClientService;

            CurrentUser = _mockDataService.GetCurrentUserInstance();
            Comments = new ObservableCollection<Comment>();
            Ratings = new ObservableCollection<Rating>();
            InstructionsWithNumbers = new ObservableCollection<NumberedInstruction>();
        }

        public ReciptDetailViewModel() : this(new MockDataService(), new HttpClientService())
        {
        }

        partial void OnRecipeIdChanged(int value)
        {
            _ = LoadRecipeDataAsync(value);
        }

        private async Task LoadRecipeDataAsync(int id)
        {
            try
            {
                if (_httpClientService != null)
                {
                    await _httpClientService.CheckApiHealthAsync();
                }

                if (_httpClientService?.IsApiAvailable == true)
                {
                    var response = await _httpClientService.GetAsync<ApiResponse<Recipe>>($"/recipes/{id}");
                    CurrentRecipe = response?.Data;
                    CurrentRecipe?.SyncFromApi();
                }
                else
                {
                    CurrentRecipe = _mockDataService.GetRecipeByIdInstance(id);
                }

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
                await Shell.Current.DisplayAlert("Error", $"Failed to load recipe: {ex.Message}", "OK");
            }
        }

        private async void LoadComments()
        {
            Comments.Clear();
            if (_httpClientService?.IsApiAvailable == true && !string.IsNullOrEmpty(CurrentRecipe?.ApiId))
            {
                try
                {
                    var response = await _httpClientService.GetAsync<ApiResponse<List<Comment>>>($"/recipes/{CurrentRecipe!.ApiId}/comments");
                    if (response?.Data != null)
                    {
                        foreach (var c in response.Data)
                        {
                            Comments.Add(c);
                        }
                    }
                }
                catch
                {
                    LoadCommentsFromMock();
                }
            }
            else
            {
                LoadCommentsFromMock();
            }
        }

        private void LoadCommentsFromMock()
        {
            var allComments = _mockDataService.GetCommentsInstance();
            var recipeComments = allComments.Where(c => c.RecipeId == CurrentRecipe!.Id).ToList();
            foreach (var comment in recipeComments)
            {
                Comments.Add(comment);
            }
        }

        private async void LoadRatings()
        {
            Ratings.Clear();
            if (_httpClientService?.IsApiAvailable == true && !string.IsNullOrEmpty(CurrentRecipe?.ApiId))
            {
                try
                {
                    var response = await _httpClientService.GetAsync<ApiResponse<List<Rating>>>($"/recipes/{CurrentRecipe!.ApiId}/ratings");
                    if (response?.Data != null)
                    {
                        foreach (var r in response.Data)
                        {
                            Ratings.Add(r);
                        }
                    }
                }
                catch
                {
                    LoadRatingsFromMock();
                }
            }
            else
            {
                LoadRatingsFromMock();
            }
        }

        private void LoadRatingsFromMock()
        {
            var allRatings = _mockDataService.GetRatingsInstance();
            var recipeRatings = allRatings.Where(r => r.RecipeId == CurrentRecipe!.Id).ToList();
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