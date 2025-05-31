namespace ReciptShare.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int TotalTimeMinutes => PrepTimeMinutes + CookTimeMinutes;
        public int Servings { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<string> Instructions { get; set; } = new List<string>();
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public double Rating { get; set; }
        public int RatingsCount { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public bool IsFavorited { get; set; }
        public bool IsLiked { get; set; }
    }

    public enum DifficultyLevel
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }
}