using ReciptShare.ViewModels;

namespace ReciptShare.Views;

public partial class EditProfilePage : ContentPage
{
    private EditProfileViewModel _viewModel;

    public EditProfilePage(EditProfileViewModel viewModel)
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