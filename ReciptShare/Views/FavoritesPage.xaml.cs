using ReciptShare.ViewModels;

namespace ReciptShare.Views;

public partial class FavoritesPage : ContentPage
{
    private FavoritesViewModel _viewModel;

    public FavoritesPage(FavoritesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh favorites when page appears (in case favorites were changed on other pages)
        _ = _viewModel.LoadFavoritesAsync();
    }
}