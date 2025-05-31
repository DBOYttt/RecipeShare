using ReciptShare.ViewModels;

namespace ReciptShare.Views;

public partial class ShoppingListPage : ContentPage
{
    private ShoppingListViewModel _viewModel;

    public ShoppingListPage()
    {
        InitializeComponent();
        _viewModel = new ShoppingListViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh shopping list when page appears
        _viewModel.LoadShoppingList();
    }
}