using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace ReciptShare.ViewModels;

public partial class WelcomeViewModel : BaseViewModel
{
    [RelayCommand]
    private async Task GoToLogin()
    {
        await Shell.Current.GoToAsync("//login");
    }

    [RelayCommand]
    private async Task GoToRegister()
    {
        await Shell.Current.GoToAsync("//register");
    }

    [RelayCommand]
    private async Task ContinueAsGuest()
    {
        await Shell.Current.GoToAsync("//home");
    }
}
