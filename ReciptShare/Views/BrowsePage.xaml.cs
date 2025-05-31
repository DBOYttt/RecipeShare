using ReciptShare.ViewModels;

namespace ReciptShare.Views;

public partial class BrowsePage : ContentPage
{
    private BrowseViewModel _viewModel;

    public BrowsePage()
    {
        InitializeComponent();
        _viewModel = new BrowseViewModel();
        BindingContext = _viewModel;
    }

    private void OnClearCategoryClicked(object sender, EventArgs e)
    {
        _viewModel.SelectedCategory = "All";
    }

    private void OnClearAllFiltersClicked(object sender, EventArgs e)
    {
        _viewModel.SearchText = string.Empty;
        _viewModel.SelectedCategory = "All";
        _viewModel.SortOption = "Latest";
    }
}