using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization; // ADD THIS LINE

namespace ReciptShare.Services
{
    public interface IHttpClientService
    {
        Task<T?> GetAsync<T>(string endpoint);
        Task<T?> PostAsync<T>(string endpoint, object? data = null);
        Task<T?> PutAsync<T>(string endpoint, object? data = null);
        Task<bool> DeleteAsync(string endpoint);
        void SetAuthToken(string token);
        void ClearAuthToken();
        bool IsApiAvailable { get; }
        Task<bool> CheckApiHealthAsync();
    }

    public class HttpClientService : IHttpClientService
    {
        private HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;
        private bool _isApiAvailable = false;

        public bool IsApiAvailable => _isApiAvailable;

        public HttpClientService()
        {
            _baseUrl = GetApiBaseUrl();
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            InitializeHttpClient();

            // Check API health on initialization
            _ = Task.Run(async () => await CheckApiHealthAsync());
        }

        private void InitializeHttpClient()
        {
            var handler = new HttpClientHandler();

#if ANDROID
            // For Android, we might need specific configuration
#endif

            _httpClient = new HttpClient(handler);
            
            // Set default headers
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            
            // Set timeout
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        private string GetApiBaseUrl()
        {
            return "http://srv12.mikr.us:30346/api";
        }

        public async Task<bool> CheckApiHealthAsync()
        {
            try
            {
                var healthUrl = $"{_baseUrl}/health";
                System.Diagnostics.Debug.WriteLine($"[API] Checking health at: {healthUrl}");
                
                var response = await _httpClient.GetAsync(healthUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"[API] Health response: {content}");
                    
                    // Parse the health response
                    var healthResponse = JsonSerializer.Deserialize<ApiHealthResponse>(content, _jsonOptions);
                    
                    // Check if the API reports healthy status
                    _isApiAvailable = healthResponse?.Status?.ToLower() == "healthy";
                    
                    System.Diagnostics.Debug.WriteLine($"[API] Health check result: {_isApiAvailable}");
                    System.Diagnostics.Debug.WriteLine($"[API] API Version: {healthResponse?.Version}");
                    System.Diagnostics.Debug.WriteLine($"[API] Environment: {healthResponse?.Environment}");
                    System.Diagnostics.Debug.WriteLine($"[API] Database: {healthResponse?.Database?.Status}");
                    
                    return _isApiAvailable;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[API] Health check failed - HTTP {response.StatusCode}");
                    _isApiAvailable = false;
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] Health check failed - HttpRequestException: {ex.Message}");
                _isApiAvailable = false;
                return false;
            }
            catch (TaskCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] Health check failed - Timeout: {ex.Message}");
                _isApiAvailable = false;
                return false;
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] Health check failed - JSON parsing error: {ex.Message}");
                _isApiAvailable = false;
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] Health check failed - Exception: {ex.Message}");
                _isApiAvailable = false;
                return false;
            }
        }

        public void SetAuthToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }

        public void ClearAuthToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var fullUrl = $"{_baseUrl}{endpoint}";
                System.Diagnostics.Debug.WriteLine($"[API] GET: {fullUrl}");
                
                var response = await _httpClient.GetAsync(fullUrl);
                _isApiAvailable = true;
                
                var content = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[API] Response: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<T>(content, _jsonOptions);
                }
                
                await HandleErrorResponse(response, content);
                return default;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] Exception: {ex.Message}");
                _isApiAvailable = false;
                throw new ApiException("API is currently unavailable. Using offline mode.");
            }
        }

        public async Task<T?> PostAsync<T>(string endpoint, object? data = null)
        {
            try
            {
                var fullUrl = $"{_baseUrl}{endpoint}";
                var json = data != null ? JsonSerializer.Serialize(data, _jsonOptions) : "";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                System.Diagnostics.Debug.WriteLine($"[API] POST: {fullUrl}");
                System.Diagnostics.Debug.WriteLine($"[API] Body: {json}");
                
                var response = await _httpClient.PostAsync(fullUrl, content);
                _isApiAvailable = true;
                
                var responseContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[API] Response: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
                }
                
                await HandleErrorResponse(response, responseContent);
                return default;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] Exception: {ex.Message}");
                _isApiAvailable = false;
                throw new ApiException("API is currently unavailable. Using offline mode.");
            }
        }

        public async Task<T?> PutAsync<T>(string endpoint, object? data = null)
        {
            try
            {
                var fullUrl = $"{_baseUrl}{endpoint}";
                var json = data != null ? JsonSerializer.Serialize(data, _jsonOptions) : "";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync(fullUrl, content);
                _isApiAvailable = true;
                
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
                }
                
                await HandleErrorResponse(response, responseContent);
                return default;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] Exception: {ex.Message}");
                _isApiAvailable = false;
                throw new ApiException("API is currently unavailable. Using offline mode.");
            }
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}{endpoint}");
                _isApiAvailable = true;
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] Exception: {ex.Message}");
                _isApiAvailable = false;
                throw new ApiException("API is currently unavailable. Using offline mode.");
            }
        }

        private async Task HandleErrorResponse(HttpResponseMessage response, string content)
        {
            try
            {
                var errorResponse = JsonSerializer.Deserialize<Models.Api.ApiErrorResponse>(content, _jsonOptions);
                throw new ApiException($"{errorResponse?.Error}: {errorResponse?.Message}");
            }
            catch (JsonException)
            {
                throw new ApiException($"API Error: {response.StatusCode} - {content}");
            }
        }
    }

    // Health Response Model
    public class ApiHealthResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("uptime")]
        public double Uptime { get; set; }

        [JsonPropertyName("environment")]
        public string Environment { get; set; } = string.Empty;

        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;

        [JsonPropertyName("database")]
        public DatabaseHealth? Database { get; set; }

        [JsonPropertyName("system")]
        public SystemHealth? System { get; set; }
    }

    public class DatabaseHealth
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("responseTime")]
        public string ResponseTime { get; set; } = string.Empty;
    }

    public class SystemHealth
    {
        [JsonPropertyName("memory")]
        public MemoryInfo? Memory { get; set; }

        [JsonPropertyName("cpu")]
        public CpuInfo? Cpu { get; set; }

        [JsonPropertyName("nodeVersion")]
        public string NodeVersion { get; set; } = string.Empty;

        [JsonPropertyName("platform")]
        public string Platform { get; set; } = string.Empty;
    }

    public class MemoryInfo
    {
        [JsonPropertyName("used")]
        public int Used { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; } = string.Empty;
    }

    public class CpuInfo
    {
        [JsonPropertyName("user")]
        public long User { get; set; }

        [JsonPropertyName("system")]
        public long System { get; set; }
    }

    public class ApiException : Exception
    {
        public ApiException(string message) : base(message) { }
        public ApiException(string message, Exception innerException) : base(message, innerException) { }
    }
}