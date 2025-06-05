using ReciptShare.ViewModels;

namespace ReciptShare.Views;

public partial class ShoppingListPage : ContentPage
{
    private ShoppingListViewModel _viewModel;

    public ShoppingListPage(ShoppingListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh shopping list when page appears
        _viewModel.LoadShoppingList();
    }
}