using ReciptShare.Models;

namespace ReciptShare.Services
{
    public class MockDataService
    {
        private static List<User> _users;
        private static List<Recipe> _recipes;
        private static List<Comment> _comments;
        private static List<Rating> _ratings;

        static MockDataService()
        {
            InitializeMockData();
        }

        public static List<User> GetUsers() => _users;
        public static List<Recipe> GetRecipes() => _recipes;
        public static List<Comment> GetComments() => _comments;
        public static List<Rating> GetRatings() => _ratings;

        public static User GetCurrentUser() => _users.First();

        public static List<Recipe> GetRecipesByCategory(string category)
        {
            return _recipes.Where(r => r.Categories.Contains(category)).ToList();
        }

        public static List<Recipe> GetPopularRecipes()
        {
            return _recipes.OrderByDescending(r => r.Rating).Take(10).ToList();
        }

        public static List<Recipe> GetLatestRecipes()
        {
            return _recipes.OrderByDescending(r => r.CreatedDate).Take(10).ToList();
        }

        public static Recipe GetRecipeById(int id)
        {
            return _recipes.FirstOrDefault(r => r.Id == id);
        }

        private static void InitializeMockData()
        {
            // Initialize Users
            _users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "DBOYttt",
                    Email = "dboy@example.com",
                    FullName = "Daniel Boy",
                    ProfileImageUrl = "https://api.dicebear.com/7.x/avataaars/png?seed=DBOYttt",
                    JoinDate = DateTime.Now.AddMonths(-6),
                    Bio = "Passionate home chef and recipe creator",
                    RecipesCount = 12,
                    FollowersCount = 245,
                    FollowingCount = 89
                },
                new User
                {
                    Id = 2,
                    Username = "ChefMaria",
                    Email = "maria@example.com",
                    FullName = "Maria Rodriguez",
                    ProfileImageUrl = "https://api.dicebear.com/7.x/avataaars/png?seed=ChefMaria",
                    JoinDate = DateTime.Now.AddYears(-1),
                    Bio = "Professional chef sharing family recipes",
                    RecipesCount = 28,
                    FollowersCount = 1250,
                    FollowingCount = 156
                },
                new User
                {
                    Id = 3,
                    Username = "HealthyEats",
                    Email = "healthy@example.com",
                    FullName = "Sarah Johnson",
                    ProfileImageUrl = "https://api.dicebear.com/7.x/avataaars/png?seed=HealthyEats",
                    JoinDate = DateTime.Now.AddMonths(-3),
                    Bio = "Nutritionist focused on healthy, delicious meals",
                    RecipesCount = 18,
                    FollowersCount = 892,
                    FollowingCount = 203
                }
            };

            // Initialize Recipes
            _recipes = new List<Recipe>
            {
                new Recipe
                {
                    Id = 1,
                    Title = "Classic Spaghetti Carbonara",
                    Description = "Authentic Italian carbonara with eggs, cheese, and pancetta",
                    ImageUrl = "https://images.unsplash.com/photo-1621996346565-e3dbc353d2e5?w=400",
                    PrepTimeMinutes = 10,
                    CookTimeMinutes = 15,
                    Servings = 4,
                    Difficulty = DifficultyLevel.Medium,
                    Categories = new List<string> { "Italian", "Pasta", "Dinner" },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Id = 1, Name = "Spaghetti", Quantity = 400, Unit = "g" },
                        new Ingredient { Id = 2, Name = "Pancetta", Quantity = 150, Unit = "g" },
                        new Ingredient { Id = 3, Name = "Eggs", Quantity = 3, Unit = "large" },
                        new Ingredient { Id = 4, Name = "Parmesan cheese", Quantity = 100, Unit = "g" },
                        new Ingredient { Id = 5, Name = "Black pepper", Quantity = 1, Unit = "tsp" }
                    },
                    Instructions = new List<string>
                    {
                        "Bring a large pot of salted water to boil",
                        "Cook spaghetti according to package directions",
                        "Meanwhile, cook pancetta until crispy",
                        "Beat eggs with grated parmesan and black pepper",
                        "Drain pasta, reserving 1 cup pasta water",
                        "Toss hot pasta with pancetta and egg mixture",
                        "Add pasta water as needed for creamy consistency",
                        "Serve immediately with extra parmesan"
                    },
                    AuthorId = 2,
                    AuthorName = "ChefMaria",
                    CreatedDate = DateTime.Now.AddDays(-5),
                    Rating = 4.8,
                    RatingsCount = 156,
                    CommentsCount = 23,
                    LikesCount = 289
                },
                new Recipe
                {
                    Id = 2,
                    Title = "Healthy Buddha Bowl",
                    Description = "Nutritious bowl with quinoa, roasted vegetables, and tahini dressing",
                    ImageUrl = "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=400",
                    PrepTimeMinutes = 20,
                    CookTimeMinutes = 25,
                    Servings = 2,
                    Difficulty = DifficultyLevel.Easy,
                    Categories = new List<string> { "Healthy", "Vegetarian", "Lunch" },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Id = 6, Name = "Quinoa", Quantity = 1, Unit = "cup" },
                        new Ingredient { Id = 7, Name = "Sweet potato", Quantity = 1, Unit = "large" },
                        new Ingredient { Id = 8, Name = "Chickpeas", Quantity = 1, Unit = "can" },
                        new Ingredient { Id = 9, Name = "Spinach", Quantity = 2, Unit = "cups" },
                        new Ingredient { Id = 10, Name = "Tahini", Quantity = 3, Unit = "tbsp" }
                    },
                    Instructions = new List<string>
                    {
                        "Cook quinoa according to package instructions",
                        "Cube and roast sweet potato at 400°F for 20 minutes",
                        "Drain and rinse chickpeas",
                        "Make tahini dressing with lemon juice and water",
                        "Assemble bowls with quinoa base",
                        "Top with roasted vegetables and chickpeas",
                        "Add fresh spinach and drizzle with dressing"
                    },
                    AuthorId = 3,
                    AuthorName = "HealthyEats",
                    CreatedDate = DateTime.Now.AddDays(-2),
                    Rating = 4.6,
                    RatingsCount = 89,
                    CommentsCount = 12,
                    LikesCount = 167
                },
                new Recipe
                {
                    Id = 3,
                    Title = "Chocolate Chip Cookies",
                    Description = "Classic homemade chocolate chip cookies that are crispy outside, chewy inside",
                    ImageUrl = "https://images.unsplash.com/photo-1499636136210-6f4ee915583e?w=400",
                    PrepTimeMinutes = 15,
                    CookTimeMinutes = 12,
                    Servings = 24,
                    Difficulty = DifficultyLevel.Easy,
                    Categories = new List<string> { "Dessert", "Baking", "Sweet" },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Id = 11, Name = "All-purpose flour", Quantity = 2.25, Unit = "cups" },
                        new Ingredient { Id = 12, Name = "Butter", Quantity = 1, Unit = "cup" },
                        new Ingredient { Id = 13, Name = "Brown sugar", Quantity = 0.75, Unit = "cup" },
                        new Ingredient { Id = 14, Name = "White sugar", Quantity = 0.75, Unit = "cup" },
                        new Ingredient { Id = 15, Name = "Chocolate chips", Quantity = 2, Unit = "cups" }
                    },
                    Instructions = new List<string>
                    {
                        "Preheat oven to 375°F",
                        "Cream butter and sugars until light and fluffy",
                        "Beat in eggs and vanilla",
                        "Gradually blend in flour",
                        "Stir in chocolate chips",
                        "Drop rounded tablespoons onto ungreased cookie sheets",
                        "Bake 9-11 minutes until golden brown",
                        "Cool on baking sheet for 2 minutes before removing"
                    },
                    AuthorId = 1,
                    AuthorName = "DBOYttt",
                    CreatedDate = DateTime.Now.AddDays(-1),
                    Rating = 4.9,
                    RatingsCount = 234,
                    CommentsCount = 45,
                    LikesCount = 512
                }
            };

            // Initialize Comments
            _comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    RecipeId = 1,
                    UserId = 1,
                    UserName = "DBOYttt",
                    UserAvatarUrl = "https://api.dicebear.com/7.x/avataaars/png?seed=DBOYttt",
                    Content = "This recipe is absolutely perfect! Made it for dinner last night and everyone loved it.",
                    CreatedDate = DateTime.Now.AddHours(-12),
                    LikesCount = 8
                },
                new Comment
                {
                    Id = 2,
                    RecipeId = 2,
                    UserId = 2,
                    UserName = "ChefMaria",
                    UserAvatarUrl = "https://api.dicebear.com/7.x/avataaars/png?seed=ChefMaria",
                    Content = "Love how healthy and filling this bowl is. Great combination of flavors!",
                    CreatedDate = DateTime.Now.AddHours(-6),
                    LikesCount = 5
                }
            };

            // Initialize Ratings
            _ratings = new List<Rating>
            {
                new Rating
                {
                    Id = 1,
                    RecipeId = 1,
                    UserId = 1,
                    UserName = "DBOYttt",
                    Stars = 5,
                    Review = "Amazing traditional recipe!",
                    CreatedDate = DateTime.Now.AddDays(-1)
                }
            };
        }
    }
}