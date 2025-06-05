using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReciptShare.Services;
using ReciptShare.Models.Api;
using Microsoft.Maui.Controls;

namespace ReciptShare.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string firstName = string.Empty;

        [ObservableProperty]
        private string lastName = string.Empty;

        [ObservableProperty]
        private string bio = string.Empty;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        private readonly IAuthenticationService _authService;
        private readonly IHttpClientService _httpClientService;

        public RegisterViewModel(IAuthenticationService authService, IHttpClientService httpClientService)
        {
            Title = "Register";
            _authService = authService;
            _httpClientService = httpClientService;
        }

        public RegisterViewModel() : this(new AuthenticationService(new HttpClientService()), new HttpClientService())
        {
        }

        [RelayCommand]
        private async Task NavigateToLogin()
        {
            await Shell.Current.GoToAsync("//login");
        }

        [RelayCommand]
        private async Task RegisterAsync()
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
                    StatusMessage = "API unavailable. Registration requires connection.";
                    return;
                }

                var request = new RegisterRequest
                {
                    Username = Username,
                    Email = Email,
                    Password = Password,
                    FirstName = FirstName,
                    LastName = string.IsNullOrWhiteSpace(LastName) ? null : LastName,
                    Bio = string.IsNullOrWhiteSpace(Bio) ? null : Bio
                };

                var user = await _authService.RegisterAsync(request);
                if (user != null)
                {
                    await Shell.Current.GoToAsync("//home");
                }
                else
                {
                    StatusMessage = "Registration failed.";
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
