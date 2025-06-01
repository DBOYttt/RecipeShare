using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Models;
using ReciptShare.Services;
using System.Collections.ObjectModel;

namespace ReciptShare.ViewModels
{
    public partial class ShoppingListViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<Ingredient> shoppingItems;

        [ObservableProperty]
        ObservableCollection<Ingredient> completedItems;

        [ObservableProperty]
        string newItemName = string.Empty;

        [ObservableProperty]
        double newItemQuantity = 1;

        [ObservableProperty]
        string newItemUnit = "pcs";

        [ObservableProperty]
        bool isAddingItem;

        [ObservableProperty]
        User currentUser;

        public List<string> CommonUnits { get; } = new List<string>
        {
            // Weight units (most common first)
            "g", "kg", "mg",
    
            // Volume units (metric)
            "ml", "l", "dl", "cl",
    
            // Cooking measurements
            "tbsp", "tsp", "dessert spoon",
    
            // Count units
            "pcs", "piece", "pieces",
            "slice", "slices",
    
            // Food-specific units
            "pinch", "dash", "handful",
            "can", "tin", "jar",
            "package", "packet", "sachet",
            "bunch", "sprig", "leaf", "leaves",
    
            // Legacy/International units
            "cup", "oz", "lb", "fl oz"
        };

        public ShoppingListViewModel()
        {
            Title = "Shopping List";
            CurrentUser = MockDataService.GetCurrentUser();
            LoadShoppingList();
        }

        public void LoadShoppingList()
        {
            try
            {
                // Get selected ingredients from all recipes
                var allRecipes = MockDataService.GetRecipes();
                var selectedIngredients = allRecipes
                    .SelectMany(r => r.Ingredients)
                    .Where(i => i.IsSelected)
                    .ToList();

                // For demo purposes, create some sample items
                var sampleItems = new List<Ingredient>
                {
                    new Ingredient { Id = 1, Name = "Milk", Quantity = 1, Unit = "l", IsSelected = false },
                    new Ingredient { Id = 2, Name = "Bread", Quantity = 1, Unit = "pcs", IsSelected = false },
                    new Ingredient { Id = 3, Name = "Eggs", Quantity = 12, Unit = "pcs", IsSelected = false }
                };

                ShoppingItems = new ObservableCollection<Ingredient>(sampleItems);
                CompletedItems = new ObservableCollection<Ingredient>();
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", $"Failed to load shopping list: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task AddNewItem()
        {
            if (string.IsNullOrWhiteSpace(NewItemName)) 
            {
                await Shell.Current.DisplayAlert("Invalid Input", "Please enter an item name.", "OK");
                return;
            }

            IsAddingItem = true;

            try
            {
                var newItem = new Ingredient
                {
                    Id = ShoppingItems.Count + CompletedItems.Count + 1,
                    Name = NewItemName.Trim(),
                    Quantity = NewItemQuantity,
                    Unit = NewItemUnit,
                    IsSelected = false
                };

                ShoppingItems.Insert(0, newItem);

                // Clear form
                NewItemName = string.Empty;
                NewItemQuantity = 1;
                NewItemUnit = "pcs";

                await Shell.Current.DisplayAlert("Added", "Item added to shopping list!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to add item: {ex.Message}", "OK");
            }
            finally
            {
                IsAddingItem = false;
            }
        }

        [RelayCommand]
        private async Task ToggleItemCompletion(Ingredient item)
        {
            if (item == null) return;

            try
            {
                if (ShoppingItems.Contains(item))
                {
                    // Mark as completed
                    ShoppingItems.Remove(item);
                    CompletedItems.Insert(0, item);
                }
                else if (CompletedItems.Contains(item))
                {
                    // Mark as uncompleted
                    CompletedItems.Remove(item);
                    ShoppingItems.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to update item: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task RemoveItem(Ingredient item)
        {
            if (item == null) return;

            var result = await Shell.Current.DisplayAlert(
                "Remove Item", 
                $"Remove '{item.DisplayText}' from your shopping list?", 
                "Remove", 
                "Cancel");

            if (result)
            {
                ShoppingItems.Remove(item);
                CompletedItems.Remove(item);
                await Shell.Current.DisplayAlert("Removed", "Item removed from shopping list!", "OK");
            }
        }

        [RelayCommand]
        private async Task ClearCompletedItems()
        {
            if (!CompletedItems.Any()) return;

            var result = await Shell.Current.DisplayAlert(
                "Clear Completed Items", 
                $"Remove all {CompletedItems.Count} completed items from your shopping list?", 
                "Clear", 
                "Cancel");

            if (result)
            {
                CompletedItems.Clear();
                await Shell.Current.DisplayAlert("Cleared", "Completed items have been removed!", "OK");
            }
        }

        [RelayCommand]
        private async Task ClearAllItems()
        {
            var totalItems = ShoppingItems.Count + CompletedItems.Count;
            if (totalItems == 0) return;

            var result = await Shell.Current.DisplayAlert(
                "Clear All Items", 
                $"Remove all {totalItems} items from your shopping list? This action cannot be undone.", 
                "Clear All", 
                "Cancel");

            if (result)
            {
                ShoppingItems.Clear();
                CompletedItems.Clear();
                await Shell.Current.DisplayAlert("Cleared", "Shopping list has been cleared!", "OK");
            }
        }

        [RelayCommand]
        private async Task ShareShoppingList()
        {
            try
            {
                if (!ShoppingItems.Any() && !CompletedItems.Any())
                {
                    await Shell.Current.DisplayAlert("Empty List", "Your shopping list is empty!", "OK");
                    return;
                }

                var listText = "Shopping List:\n\n";
                
                if (ShoppingItems.Any())
                {
                    listText += "To Buy:\n";
                    foreach (var item in ShoppingItems)
                    {
                        listText += $"• {item.DisplayText}\n";
                    }
                    listText += "\n";
                }

                if (CompletedItems.Any())
                {
                    listText += "Completed:\n";
                    foreach (var item in CompletedItems)
                    {
                        listText += $"✓ {item.DisplayText}\n";
                    }
                }

                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = listText,
                    Title = "Share Shopping List"
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to share shopping list: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task BrowseRecipes()
        {
            await Shell.Current.GoToAsync("//browse");
        }
    }
}