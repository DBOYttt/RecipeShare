using ReciptShare.ViewModels;

namespace ReciptShare.Views;

public partial class ProfilePage : ContentPage
{
    private ProfileViewModel _viewModel;

    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh profile when page appears
        _viewModel.RefreshProfileCommand.Execute(null);
    }
}