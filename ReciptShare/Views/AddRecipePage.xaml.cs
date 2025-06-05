using ReciptShare.ViewModels;

namespace ReciptShare.Views;

public partial class AddRecipePage : ContentPage
{
    private AddReciptViewModel _viewModel;

    public AddRecipePage(AddReciptViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override bool OnBackButtonPressed()
    {
        // Handle back button to show confirmation dialog
        _viewModel.GoBackCommand.Execute(null);
        return true; // Prevent default back navigation
    }
}