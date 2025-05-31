using ReciptShare.ViewModels;

namespace ReciptShare.Views;

public partial class FavoritesPage : ContentPage
{
    private FavoritesViewModel _viewModel;

    public FavoritesPage()
    {
        InitializeComponent();
        _viewModel = new FavoritesViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh favorites when page appears (in case favorites were changed on other pages)
        _viewModel.LoadFavorites();
    }
}