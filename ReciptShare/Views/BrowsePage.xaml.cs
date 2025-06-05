using ReciptShare.ViewModels;

namespace ReciptShare.Views;

public partial class BrowsePage : ContentPage
{
    private BrowseViewModel _viewModel;

    public BrowsePage(BrowseViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
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