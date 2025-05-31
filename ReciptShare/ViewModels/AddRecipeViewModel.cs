using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Models;
using ReciptShare.Services;
using System.Collections.ObjectModel;

namespace ReciptShare.ViewModels
{
    public partial class AddRecipeViewModel : BaseViewModel
    {
        [ObservableProperty]
        string recipeTitle = string.Empty;

        [ObservableProperty]
        string recipeDescription = string.Empty;

        [ObservableProperty]
        int prepTimeMinutes = 15;

        [ObservableProperty]
        int cookTimeMinutes = 30;

        [ObservableProperty]
        int servings = 4;

        [ObservableProperty]
        DifficultyLevel difficulty = DifficultyLevel.Easy;

        [ObservableProperty]
        string selectedCategory = "Main Course";

        [ObservableProperty]
        ObservableCollection<string> selectedCategories;

        [ObservableProperty]
        ObservableCollection<Ingredient> ingredients;

        [ObservableProperty]
        ObservableCollection<string> instructions;

        [ObservableProperty]
        string newIngredientName = string.Empty;

        [ObservableProperty]
        double newIngredientQuantity = 1;

        [ObservableProperty]
        string newIngredientUnit = "cup";

        [ObservableProperty]
        string newInstruction = string.Empty;

        [ObservableProperty]
        string imageUrl = string.Empty;

        [ObservableProperty]
        bool isSaving;

        [ObservableProperty]
        User currentUser;

        [ObservableProperty]
        string currentStep = "Basic Info";

        // Computed property for display
        public int TotalTimeMinutes => PrepTimeMinutes + CookTimeMinutes;

        // Convert enum to list of strings for the picker
        public List<string> DifficultyLevels { get; } = Enum.GetNames(typeof(DifficultyLevel)).ToList();

        // Selected difficulty as string for binding
        public string SelectedDifficulty
        {
            get => Difficulty.ToString();
            set
            {
                if (Enum.TryParse<DifficultyLevel>(value, out var difficultyLevel))
                {
                    Difficulty = difficultyLevel;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Difficulty));
                }
            }
        }

        public List<string> Categories { get; } = new List<string>
        {
            "Appetizers", "Main Course", "Desserts", "Beverages", "Breakfast", "Lunch", "Dinner",
            "Snacks", "Vegetarian", "Vegan", "Gluten-Free", "Low-Carb", "Italian", "Mexican",
            "Asian", "Indian", "Mediterranean", "American", "French", "Thai"
        };

        public List<string> CommonUnits { get; } = new List<string>
        {
            "cup", "cups", "tbsp", "tsp", "oz", "lb", "g", "kg", "ml", "l", "qt", "gal",
            "piece", "pieces", "slice", "slices", "clove", "cloves", "can", "package", "bunch"
        };

        public List<string> FormSteps { get; } = new List<string>
        {
            "Basic Info", "Ingredients", "Instructions", "Categories", "Final Review"
        };

        public AddRecipeViewModel()
        {
            Title = "Add New Recipe";
            CurrentUser = MockDataService.GetCurrentUser();
            SelectedCategories = new ObservableCollection<string>();
            Ingredients = new ObservableCollection<Ingredient>();
            Instructions = new ObservableCollection<string>();
        }

        // Update total time when prep or cook time changes
        partial void OnPrepTimeMinutesChanged(int value)
        {
            OnPropertyChanged(nameof(TotalTimeMinutes));
        }

        partial void OnCookTimeMinutesChanged(int value)
        {
            OnPropertyChanged(nameof(TotalTimeMinutes));
        }

        [RelayCommand]
        private async Task AddIngredient()
        {
            if (string.IsNullOrWhiteSpace(NewIngredientName))
            {
                await Shell.Current.DisplayAlert("Invalid Input", "Please enter an ingredient name.", "OK");
                return;
            }

            var ingredient = new Ingredient
            {
                Id = Ingredients.Count + 1,
                Name = NewIngredientName.Trim(),
                Quantity = NewIngredientQuantity,
                Unit = NewIngredientUnit,
                IsSelected = false
            };

            Ingredients.Add(ingredient);

            // Clear form
            NewIngredientName = string.Empty;
            NewIngredientQuantity = 1;
            NewIngredientUnit = "cup";
        }

        [RelayCommand]
        private async Task RemoveIngredient(Ingredient ingredient)
        {
            if (ingredient != null)
            {
                Ingredients.Remove(ingredient);
            }
        }

        [RelayCommand]
        private async Task EditIngredient(Ingredient ingredient)
        {
            if (ingredient == null) return;

            var action = await Shell.Current.DisplayActionSheet(
                $"Edit: {ingredient.DisplayText}",
                "Cancel",
                "Delete",
                "Edit Name",
                "Edit Quantity",
                "Edit Unit");

            switch (action)
            {
                case "Edit Name":
                    var newName = await Shell.Current.DisplayPromptAsync("Edit Name", "Enter new name:", initialValue: ingredient.Name);
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        ingredient.Name = newName.Trim();
                    }
                    break;
                case "Edit Quantity":
                    var newQuantity = await Shell.Current.DisplayPromptAsync("Edit Quantity", "Enter new quantity:", initialValue: ingredient.Quantity.ToString(), keyboard: Keyboard.Numeric);
                    if (double.TryParse(newQuantity, out double qty))
                    {
                        ingredient.Quantity = qty;
                    }
                    break;
                case "Edit Unit":
                    var newUnit = await Shell.Current.DisplayActionSheet("Select Unit", "Cancel", null, CommonUnits.ToArray());
                    if (!string.IsNullOrEmpty(newUnit) && newUnit != "Cancel")
                    {
                        ingredient.Unit = newUnit;
                    }
                    break;
                case "Delete":
                    Ingredients.Remove(ingredient);
                    break;
            }
        }

        [RelayCommand]
        private async Task AddInstruction()
        {
            if (string.IsNullOrWhiteSpace(NewInstruction))
            {
                await Shell.Current.DisplayAlert("Invalid Input", "Please enter an instruction.", "OK");
                return;
            }

            Instructions.Add(NewInstruction.Trim());
            NewInstruction = string.Empty;
        }

        [RelayCommand]
        private async Task RemoveInstruction(string instruction)
        {
            if (!string.IsNullOrEmpty(instruction))
            {
                Instructions.Remove(instruction);
            }
        }

        [RelayCommand]
        private async Task EditInstruction(string instruction)
        {
            if (string.IsNullOrEmpty(instruction)) return;

            var index = Instructions.IndexOf(instruction);
            if (index >= 0)
            {
                var newInstruction = await Shell.Current.DisplayPromptAsync(
                    $"Edit Step {index + 1}",
                    "Edit instruction:",
                    initialValue: instruction);

                if (!string.IsNullOrWhiteSpace(newInstruction))
                {
                    Instructions[index] = newInstruction.Trim();
                }
            }
        }

        [RelayCommand]
        private async Task MoveInstructionUp(string instruction)
        {
            var index = Instructions.IndexOf(instruction);
            if (index > 0)
            {
                Instructions.Move(index, index - 1);
            }
        }

        [RelayCommand]
        private async Task MoveInstructionDown(string instruction)
        {
            var index = Instructions.IndexOf(instruction);
            if (index >= 0 && index < Instructions.Count - 1)
            {
                Instructions.Move(index, index + 1);
            }
        }

        [RelayCommand]
        private async Task ToggleCategory(string category)
        {
            if (SelectedCategories.Contains(category))
            {
                SelectedCategories.Remove(category);
            }
            else
            {
                SelectedCategories.Add(category);
            }
        }

        [RelayCommand]
        private async Task NextStep()
        {
            var currentIndex = FormSteps.IndexOf(CurrentStep);
            if (currentIndex < FormSteps.Count - 1)
            {
                CurrentStep = FormSteps[currentIndex + 1];
            }
        }

        [RelayCommand]
        private async Task PreviousStep()
        {
            var currentIndex = FormSteps.IndexOf(CurrentStep);
            if (currentIndex > 0)
            {
                CurrentStep = FormSteps[currentIndex - 1];
            }
        }

        [RelayCommand]
        private async Task GoToStep(string step)
        {
            if (FormSteps.Contains(step))
            {
                CurrentStep = step;
            }
        }

        [RelayCommand]
        private async Task SaveRecipe()
        {
            if (!ValidateRecipe())
            {
                return;
            }

            IsSaving = true;

            try
            {
                var newRecipe = new Recipe
                {
                    Id = MockDataService.GetRecipes().Count + 1,
                    Title = RecipeTitle.Trim(),
                    Description = RecipeDescription.Trim(),
                    AuthorId = CurrentUser.Id,
                    AuthorName = CurrentUser.FullName,
                    PrepTimeMinutes = PrepTimeMinutes,
                    CookTimeMinutes = CookTimeMinutes,
                    // Don't set TotalTimeMinutes - it's calculated automatically
                    Servings = Servings,
                    Difficulty = Difficulty, // Use the enum directly
                    Categories = SelectedCategories.ToList(),
                    Ingredients = Ingredients.ToList(),
                    Instructions = Instructions.ToList(),
                    ImageUrl = ImageUrl.Trim(),
                    CreatedDate = DateTime.Now,
                    Rating = 0,
                    RatingsCount = 0,
                    LikesCount = 0,
                    CommentsCount = 0,
                    IsFavorited = false
                };

                // Simulate saving to backend
                await Task.Delay(2000);

                // In a real app, you would save to your backend here
                // await _recipeService.SaveRecipeAsync(newRecipe);

                await Shell.Current.DisplayAlert("Success!", 
                    $"'{RecipeTitle}' has been added successfully! 🎉\n\n" +
                    "Your recipe is now available for the community to discover and enjoy!", "Awesome!");

                // Navigate back to profile or home
                await Shell.Current.GoToAsync("//profile");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to save recipe: {ex.Message}", "OK");
            }
            finally
            {
                IsSaving = false;
            }
        }

        private bool ValidateRecipe()
        {
            if (string.IsNullOrWhiteSpace(RecipeTitle))
            {
                Shell.Current.DisplayAlert("Validation Error", "Please enter a recipe title.", "OK");
                CurrentStep = "Basic Info";
                return false;
            }

            if (string.IsNullOrWhiteSpace(RecipeDescription))
            {
                Shell.Current.DisplayAlert("Validation Error", "Please enter a recipe description.", "OK");
                CurrentStep = "Basic Info";
                return false;
            }

            if (!Ingredients.Any())
            {
                Shell.Current.DisplayAlert("Validation Error", "Please add at least one ingredient.", "OK");
                CurrentStep = "Ingredients";
                return false;
            }

            if (!Instructions.Any())
            {
                Shell.Current.DisplayAlert("Validation Error", "Please add at least one instruction step.", "OK");
                CurrentStep = "Instructions";
                return false;
            }

            if (!SelectedCategories.Any())
            {
                Shell.Current.DisplayAlert("Validation Error", "Please select at least one category.", "OK");
                CurrentStep = "Categories";
                return false;
            }

            return true;
        }

        [RelayCommand]
        private async Task ClearForm()
        {
            var result = await Shell.Current.DisplayAlert("Clear Form", 
                "Are you sure you want to clear all entered data? This action cannot be undone.", 
                "Clear", "Cancel");

            if (result)
            {
                RecipeTitle = string.Empty;
                RecipeDescription = string.Empty;
                PrepTimeMinutes = 15;
                CookTimeMinutes = 30;
                Servings = 4;
                Difficulty = DifficultyLevel.Easy;
                ImageUrl = string.Empty;
                SelectedCategories.Clear();
                Ingredients.Clear();
                Instructions.Clear();
                CurrentStep = "Basic Info";

                await Shell.Current.DisplayAlert("Cleared", "Form has been cleared successfully.", "OK");
            }
        }

        [RelayCommand]
        private async Task SaveAsDraft()
        {
            await Shell.Current.DisplayAlert("Save as Draft", 
                "Draft functionality will be available in a future update!\n\n" +
                "For now, please complete and save your recipe.", "OK");
        }

        [RelayCommand]
        private async Task PreviewRecipe()
        {
            if (string.IsNullOrWhiteSpace(RecipeTitle))
            {
                await Shell.Current.DisplayAlert("Preview", "Please add a recipe title to preview.", "OK");
                return;
            }

            var preview = $"📋 Recipe Preview\n\n" +
                         $"Title: {RecipeTitle}\n" +
                         $"Description: {RecipeDescription}\n" +
                         $"Prep Time: {PrepTimeMinutes} min\n" +
                         $"Cook Time: {CookTimeMinutes} min\n" +
                         $"Total Time: {TotalTimeMinutes} min\n" +
                         $"Servings: {Servings}\n" +
                         $"Difficulty: {Difficulty}\n" +
                         $"Categories: {string.Join(", ", SelectedCategories)}\n" +
                         $"Ingredients: {Ingredients.Count}\n" +
                         $"Instructions: {Instructions.Count} steps";

            await Shell.Current.DisplayAlert("Recipe Preview", preview, "OK");
        }

        [RelayCommand]
        private async Task GoBack()
        {
            var hasData = !string.IsNullOrWhiteSpace(RecipeTitle) || 
                         !string.IsNullOrWhiteSpace(RecipeDescription) || 
                         Ingredients.Any() || 
                         Instructions.Any();

            if (hasData)
            {
                var result = await Shell.Current.DisplayAlert("Discard Changes", 
                    "You have unsaved changes. Are you sure you want to go back?", 
                    "Discard", "Stay");

                if (!result) return;
            }

            await Shell.Current.GoToAsync("..");
        }
    }
}