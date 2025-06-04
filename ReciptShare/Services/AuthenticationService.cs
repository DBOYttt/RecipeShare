using ReciptShare.Models;
using ReciptShare.Models.Api;

namespace ReciptShare.Services
{
    public interface IAuthenticationService
    {
        Task<User?> LoginAsync(string emailOrUsername, string password);
        Task<User?> RegisterAsync(RegisterRequest request);
        Task LogoutAsync();
        Task<User?> GetCurrentUserAsync();
        bool IsAuthenticated { get; }
        string? AuthToken { get; }
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientService _httpClient;
        private string? _authToken;
        private User? _currentUser;

        public bool IsAuthenticated => !string.IsNullOrEmpty(_authToken) && _currentUser != null;
        public string? AuthToken => _authToken;

        public AuthenticationService(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
            LoadStoredAuth();
        }

        public async Task<User?> LoginAsync(string emailOrUsername, string password)
        {
            try
            {
                var request = new LoginRequest
                {
                    EmailOrUsername = emailOrUsername,
                    Password = password
                };

                var response = await _httpClient.PostAsync<ApiResponse<AuthResponse>>("/auth/login", request);
                
                if (response?.Data != null)
                {
                    _authToken = response.Data.Token;
                    _currentUser = response.Data.User;
                    
                    _httpClient.SetAuthToken(_authToken);
                    await StoreAuthAsync(_authToken, _currentUser);
                    
                    return _currentUser;
                }
                
                return null;
            }
            catch (ApiException)
            {
                // API unavailable - could implement offline login here
                throw;
            }
        }

        public async Task<User?> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsync<ApiResponse<AuthResponse>>("/auth/register", request);
                
                if (response?.Data != null)
                {
                    _authToken = response.Data.Token;
                    _currentUser = response.Data.User;
                    
                    _httpClient.SetAuthToken(_authToken);
                    await StoreAuthAsync(_authToken, _currentUser);
                    
                    return _currentUser;
                }
                
                return null;
            }
            catch (ApiException)
            {
                throw;
            }
        }

        public async Task LogoutAsync()
        {
            _authToken = null;
            _currentUser = null;
            
            _httpClient.ClearAuthToken();
            await ClearStoredAuthAsync();
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            return _currentUser;
        }

        private void LoadStoredAuth()
        {
            try
            {
                // Load from secure storage
                var storedToken = Preferences.Get("auth_token", string.Empty);
                var storedUserJson = Preferences.Get("current_user", string.Empty);

                if (!string.IsNullOrEmpty(storedToken) && !string.IsNullOrEmpty(storedUserJson))
                {
                    _authToken = storedToken;
                    _currentUser = System.Text.Json.JsonSerializer.Deserialize<User>(storedUserJson);
                    _httpClient.SetAuthToken(_authToken);
                }
            }
            catch (Exception)
            {
                // If loading fails, just continue without stored auth
            }
        }

        private async Task StoreAuthAsync(string token, User user)
        {
            try
            {
                Preferences.Set("auth_token", token);
                var userJson = System.Text.Json.JsonSerializer.Serialize(user);
                Preferences.Set("current_user", userJson);
            }
            catch (Exception)
            {
                // If storing fails, continue without persistence
            }
        }

        private async Task ClearStoredAuthAsync()
        {
            try
            {
                Preferences.Remove("auth_token");
                Preferences.Remove("current_user");
            }
            catch (Exception)
            {
                // If clearing fails, just continue
            }
        }
    }
}