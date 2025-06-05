using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Services;
using ReciptShare.Models.Api;
using Microsoft.Maui.Controls;

namespace ReciptShare.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string emailOrUsername = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        private readonly IAuthenticationService _authService;
        private readonly IHttpClientService _httpClientService;

        public LoginViewModel(IAuthenticationService authService, IHttpClientService httpClientService)
        {
            Title = "Login";
            _authService = authService;
            _httpClientService = httpClientService;
        }

        // Parameterless constructor for XAML preview/fallback
        public LoginViewModel() : this(new AuthenticationService(new HttpClientService()), new HttpClientService())
        {
        }

        [RelayCommand]
        private async Task NavigateToRegister()
        {
            await Shell.Current.GoToAsync("//register");
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await _httpClientService.CheckApiHealthAsync();
                SetApiStatus(_httpClientService.IsApiAvailable);

                if (!_httpClientService.IsApiAvailable)
                {
                    StatusMessage = "API unavailable. Login requires connection.";
                    return;
                }

                var user = await _authService.LoginAsync(EmailOrUsername, Password);
                if (user != null)
                {
                    await Shell.Current.GoToAsync("//home");
                }
                else
                {
                    StatusMessage = "Invalid credentials.";
                }
            }
            catch (ApiException ex)
            {
                StatusMessage = ex.Message;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
