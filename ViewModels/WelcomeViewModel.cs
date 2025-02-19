using System.Windows.Input;
using Microsoft.Maui.Controls;
using RecipeShare.Services;

namespace RecipeShare.ViewModels
{
  public class WelcomeViewModel : BaseViewModel
  {
    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    public string DemoMessage { get; }

    public WelcomeViewModel()
    {
      DemoMessage = App.IsDemoMode ? "Jesteś offline, wersja demo" : string.Empty;

      LoginCommand = new Command(OnLogin);
      RegisterCommand = new Command(OnRegister);
    }

    private async void OnLogin()
    {
      await Shell.Current.GoToAsync("LoginPage");
    }

    private async void OnRegister()
    {
      await Shell.Current.GoToAsync("RegisterPage");
    }
  }
}
