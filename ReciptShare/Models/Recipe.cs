using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace ReciptShare.Models
{
    public partial class Recipe : ObservableObject
    {
        // Original properties for backward compatibility
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string AuthorAvatarUrl { get; set; } = string.Empty;
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        
        // Make TotalTimeMinutes calculated property with setter for backwards compatibility
        public int TotalTimeMinutes 
        { 
            get => PrepTimeMinutes + CookTimeMinutes;
            set { } // Empty setter for compatibility - value is calculated
        }
        
        public int Servings { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<string> Instructions { get; set; } = new List<string>();
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public double Rating { get; set; }
        public int RatingsCount { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsFavorited { get; set; }

        // New API-specific properties with JsonPropertyName attributes
        [JsonPropertyName("id")]
        public string ApiId { get; set; } = string.Empty;

        [JsonPropertyName("prepTimeMinutes")]
        public int ApiPrepTimeMinutes 
        { 
            get => PrepTimeMinutes; 
            set => PrepTimeMinutes = value; 
        }

        [JsonPropertyName("cookTimeMinutes")]
        public int ApiCookTimeMinutes 
        { 
            get => CookTimeMinutes; 
            set => CookTimeMinutes = value; 
        }

        [JsonPropertyName("totalTimeMinutes")]
        public int ApiTotalTimeMinutes 
        { 
            get => TotalTimeMinutes; 
            set { } // Calculated property
        }

        [JsonPropertyName("servings")]
        public int ApiServings 
        { 
            get => Servings; 
            set => Servings = value; 
        }

        [JsonPropertyName("difficulty")]
        public string ApiDifficulty 
        { 
            get => Difficulty.ToString(); 
            set 
            {
                if (Enum.TryParse<DifficultyLevel>(value, true, out var difficulty))
                    Difficulty = difficulty;
            }
        }

        [JsonPropertyName("imageUrl")]
        public string? ApiImageUrl 
        { 
            get => string.IsNullOrEmpty(ImageUrl) ? null : ImageUrl; 
            set => ImageUrl = value ?? string.Empty; 
        }

        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("isFeatured")]
        public bool IsFeatured { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime ApiCreatedAt 
        { 
            get => CreatedDate; 
            set => CreatedDate = value; 
        }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("author")]
        public RecipeAuthor? Author { get; set; }

        [JsonPropertyName("categories")]
        public List<RecipeCategory> ApiCategories { get; set; } = new();

        [JsonPropertyName("stats")]
        public RecipeStats? Stats { get; set; }

        [JsonPropertyName("isLikedByUser")]
        public bool IsLikedByUser 
        { 
            get => IsFavorited; 
            set => IsFavorited = value; 
        }

        // Constructor to sync API data with legacy properties
        public void SyncFromApi()
        {
            if (!string.IsNullOrEmpty(ApiId))
            {
                // Convert string ID to int for legacy compatibility
                if (int.TryParse(ApiId, out int intId))
                    Id = intId;
                else
                    Id = ApiId.GetHashCode(); // Fallback for GUID IDs
            }

            if (Author != null)
            {
                AuthorName = Author.FullName;
                AuthorAvatarUrl = Author.ProfileImageUrl;
                // Convert author ID if needed
                if (int.TryParse(Author.Id, out int authorIntId))
                    AuthorId = authorIntId;
            }

            if (ApiCategories?.Any() == true)
            {
                Categories = ApiCategories.Select(c => c.Name).ToList();
            }

            if (Stats != null)
            {
                LikesCount = Stats.LikesCount;
                CommentsCount = Stats.CommentsCount;
                Rating = Stats.AverageRating ?? 0.0;
                RatingsCount = Stats.RatingsCount;
            }
        }
    }

    public enum DifficultyLevel
    {
        Easy = 1,
        Medium = 2,
        Hard = 3,
        Expert = 4
    }

    // API-specific classes
    public class RecipeAuthor
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("profileImageUrl")]
        public string ProfileImageUrl { get; set; } = string.Empty;
    }

    public class RecipeCategory
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("color")]
        public string Color { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
    }

    public class RecipeStats
    {
        [JsonPropertyName("likesCount")]
        public int LikesCount { get; set; }

        [JsonPropertyName("commentsCount")]
        public int CommentsCount { get; set; }

        [JsonPropertyName("averageRating")]
        public double? AverageRating { get; set; }

        [JsonPropertyName("ratingsCount")]
        public int RatingsCount { get; set; }
    }

    // API Response wrapper for recipe lists
    public class RecipeListResponse
    {
        [JsonPropertyName("recipes")]
        public List<Recipe> Recipes { get; set; } = new();

        [JsonPropertyName("pagination")]
        public PaginationInfo Pagination { get; set; } = new();

        [JsonPropertyName("filters")]
        public FilterInfo Filters { get; set; } = new();

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public class PaginationInfo
    {
        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("totalRecipes")]
        public int TotalRecipes { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }

        [JsonPropertyName("hasPrevPage")]
        public bool HasPrevPage { get; set; }
    }

    public class FilterInfo
    {
        [JsonPropertyName("search")]
        public string Search { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("difficulty")]
        public string Difficulty { get; set; } = string.Empty;

        [JsonPropertyName("authorId")]
        public string AuthorId { get; set; } = string.Empty;

        [JsonPropertyName("featured")]
        public string Featured { get; set; } = string.Empty;

        [JsonPropertyName("sort")]
        public string Sort { get; set; } = string.Empty;

        [JsonPropertyName("order")]
        public string Order { get; set; } = string.Empty;
    }
}