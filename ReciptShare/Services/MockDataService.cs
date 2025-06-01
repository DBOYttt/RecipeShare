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
                    Email = "dboy@diboy.org",
                    FullName = "Andrzej",
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
            Description = "Authentic Italian pasta dish with eggs, cheese, and pancetta",
            AuthorId = 1,
            AuthorName = "Marco Rossi",
            PrepTimeMinutes = 10,
            CookTimeMinutes = 15,
            Servings = 4,
            Difficulty = DifficultyLevel.Medium,
            Categories = new List<string> { "Italian", "Main Course", "Pasta" },
            Ingredients = new List<Ingredient>
            {
                new Ingredient { Id = 1, Name = "Spaghetti", Quantity = 400, Unit = "g" },
                new Ingredient { Id = 2, Name = "Pancetta", Quantity = 150, Unit = "g" },
                new Ingredient { Id = 3, Name = "Eggs", Quantity = 3, Unit = "pieces" },
                new Ingredient { Id = 4, Name = "Parmesan cheese", Quantity = 100, Unit = "g" },
                new Ingredient { Id = 5, Name = "Black pepper", Quantity = 1, Unit = "tsp" },
                new Ingredient { Id = 6, Name = "Salt", Quantity = 1, Unit = "pinch" }
            },
            Instructions = new List<string>
            {
                "Bring a large pot of salted water to boil and cook spaghetti according to package directions.",
                "While pasta cooks, heat a large skillet over medium heat and cook pancetta until crispy.",
                "In a bowl, whisk together eggs, grated Parmesan, and black pepper.",
                "Drain pasta, reserving 250ml of pasta water.",
                "Add hot pasta to pancetta pan and toss.",
                "Remove from heat and quickly stir in egg mixture, adding pasta water as needed.",
                "Serve immediately with extra Parmesan and pepper."
            },
            ImageUrl = "",
            CreatedDate = DateTime.Now.AddDays(-5),
            Rating = 4.8,
            RatingsCount = 156,
            LikesCount = 89,
            CommentsCount = 23,
            IsFavorited = true
        },
        new Recipe
        {
            Id = 2,
            Title = "Mediterranean Chicken Salad",
            Description = "Fresh and healthy salad with grilled chicken and Mediterranean flavors",
            AuthorId = 2,
            AuthorName = "Sofia Papadopoulos",
            PrepTimeMinutes = 20,
            CookTimeMinutes = 15,
            Servings = 2,
            Difficulty = DifficultyLevel.Easy,
            Categories = new List<string> { "Mediterranean", "Salad", "Healthy", "Gluten-Free" },
            Ingredients = new List<Ingredient>
            {
                new Ingredient { Id = 7, Name = "Chicken breast", Quantity = 300, Unit = "g" },
                new Ingredient { Id = 8, Name = "Mixed greens", Quantity = 100, Unit = "g" },
                new Ingredient { Id = 9, Name = "Cherry tomatoes", Quantity = 200, Unit = "g" },
                new Ingredient { Id = 10, Name = "Cucumber", Quantity = 1, Unit = "piece" },
                new Ingredient { Id = 11, Name = "Feta cheese", Quantity = 100, Unit = "g" },
                new Ingredient { Id = 12, Name = "Olives", Quantity = 50, Unit = "g" },
                new Ingredient { Id = 13, Name = "Olive oil", Quantity = 3, Unit = "tbsp" },
                new Ingredient { Id = 14, Name = "Lemon juice", Quantity = 2, Unit = "tbsp" },
                new Ingredient { Id = 15, Name = "Oregano", Quantity = 1, Unit = "tsp" }
            },
            Instructions = new List<string>
            {
                "Season chicken breast with salt, pepper, and oregano.",
                "Grill chicken for 6-7 minutes per side until cooked through.",
                "Let chicken rest for 5 minutes, then slice.",
                "In a large bowl, combine mixed greens, halved cherry tomatoes, and diced cucumber.",
                "Crumble feta cheese and add olives to the salad.",
                "Whisk together olive oil, lemon juice, oregano, salt, and pepper for dressing.",
                "Top salad with sliced chicken and drizzle with dressing."
            },
            ImageUrl = "",
            CreatedDate = DateTime.Now.AddDays(-3),
            Rating = 4.6,
            RatingsCount = 89,
            LikesCount = 67,
            CommentsCount = 15,
            IsFavorited = false
        },
        new Recipe
        {
            Id = 3,
            Title = "Chocolate Chip Cookies",
            Description = "Classic homemade cookies that are crispy on the outside and chewy on the inside",
            AuthorId = 3,
            AuthorName = "Emma Thompson",
            PrepTimeMinutes = 15,
            CookTimeMinutes = 12,
            Servings = 24,
            Difficulty = DifficultyLevel.Easy,
            Categories = new List<string> { "Desserts", "Cookies", "Baking" },
            Ingredients = new List<Ingredient>
            {
                new Ingredient { Id = 16, Name = "Plain flour", Quantity = 225, Unit = "g" },
                new Ingredient { Id = 17, Name = "Butter", Quantity = 115, Unit = "g" },
                new Ingredient { Id = 18, Name = "Brown sugar", Quantity = 100, Unit = "g" },
                new Ingredient { Id = 19, Name = "Caster sugar", Quantity = 50, Unit = "g" },
                new Ingredient { Id = 20, Name = "Egg", Quantity = 1, Unit = "piece" },
                new Ingredient { Id = 21, Name = "Vanilla extract", Quantity = 1, Unit = "tsp" },
                new Ingredient { Id = 22, Name = "Baking soda", Quantity = 1, Unit = "tsp" },
                new Ingredient { Id = 23, Name = "Salt", Quantity = 1, Unit = "pinch" },
                new Ingredient { Id = 24, Name = "Chocolate chips", Quantity = 175, Unit = "g" }
            },
            Instructions = new List<string>
            {
                "Preheat oven to 190°C (375°F). Line baking sheets with parchment paper.",
                "In a bowl, cream together butter, brown sugar, and caster sugar until light and fluffy.",
                "Beat in egg and vanilla extract.",
                "In a separate bowl, whisk together flour, baking soda, and salt.",
                "Gradually mix dry ingredients into wet ingredients.",
                "Fold in chocolate chips.",
                "Drop rounded tablespoons of dough onto prepared baking sheets, spacing 5cm apart.",
                "Bake for 10-12 minutes until edges are golden brown.",
                "Cool on baking sheet for 5 minutes before transferring to wire rack."
            },
            ImageUrl = "",
            CreatedDate = DateTime.Now.AddDays(-1),
            Rating = 4.9,
            RatingsCount = 234,
            LikesCount = 198,
            CommentsCount = 45,
            IsFavorited = true
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
                    UserAvatarUrl = "https://api.dicebear.com/7.x/avataaars/png",
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