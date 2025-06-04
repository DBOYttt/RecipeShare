using System.Text.Json.Serialization;

namespace ReciptShare.Models.Api
{
    // Base API Response
    public class ApiResponse<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public class ApiErrorResponse
    {
        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("details")]
        public object? Details { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    // Authentication Models
    public class LoginRequest
    {
        [JsonPropertyName("emailOrUsername")]
        public string EmailOrUsername { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("bio")]
        public string? Bio { get; set; }

        [JsonPropertyName("profileImageUrl")]
        public string? ProfileImageUrl { get; set; }
    }

    public class AuthResponse
    {
        [JsonPropertyName("user")]
        public User User { get; set; } = new();

        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("expiresIn")]
        public string ExpiresIn { get; set; } = string.Empty;
    }

    // Recipe API Models
    public class CreateRecipeRequest
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("prepTimeMinutes")]
        public int PrepTimeMinutes { get; set; }

        [JsonPropertyName("cookTimeMinutes")]
        public int CookTimeMinutes { get; set; }

        [JsonPropertyName("servings")]
        public int Servings { get; set; }

        [JsonPropertyName("difficulty")]
        public string Difficulty { get; set; } = string.Empty;

        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("instructions")]
        public List<string> Instructions { get; set; } = new();

        [JsonPropertyName("ingredients")]
        public List<CreateIngredientRequest> Ingredients { get; set; } = new();

        [JsonPropertyName("categoryIds")]
        public List<int>? CategoryIds { get; set; }

        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; } = true;
    }

    public class CreateIngredientRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public double Quantity { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; } = string.Empty;
    }

    // Pagination Response
    public class PaginatedResponse<T>
    {
        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = new();

        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
    }

    // Shopping List Models
    public class AddShoppingListItemRequest
    {
        [JsonPropertyName("ingredientName")]
        public string IngredientName { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public double? Quantity { get; set; }

        [JsonPropertyName("unit")]
        public string? Unit { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }

        [JsonPropertyName("recipeId")]
        public string? RecipeId { get; set; }
    }

    public class RateRecipeRequest
    {
        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("review")]
        public string? Review { get; set; }
    }

    public class AddCommentRequest
    {
        [JsonPropertyName("comment")]
        public string Comment { get; set; } = string.Empty;

        [JsonPropertyName("parentCommentId")]
        public string? ParentCommentId { get; set; }
    }
}