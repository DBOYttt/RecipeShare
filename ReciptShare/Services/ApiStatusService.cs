namespace ReciptShare.Services
{
    public interface IApiStatusService
    {
        bool IsApiAvailable { get; }
        event EventHandler<bool> ApiStatusChanged;
        Task CheckApiStatusAsync();
        void SetApiStatus(bool isAvailable);
    }

    public class ApiStatusService : IApiStatusService
    {
        private bool _isApiAvailable = true;
        
        public bool IsApiAvailable 
        { 
            get => _isApiAvailable;
            private set
            {
                if (_isApiAvailable != value)
                {
                    _isApiAvailable = value;
                    ApiStatusChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<bool>? ApiStatusChanged;

        public async Task CheckApiStatusAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                
#if ANDROID
                var response = await client.GetAsync("http://10.0.2.2:3000/api/health");
#else
                var response = await client.GetAsync("http://localhost:3000/api/health");
#endif
                SetApiStatus(response.IsSuccessStatusCode);
            }
            catch
            {
                SetApiStatus(false);
            }
        }

        public void SetApiStatus(bool isAvailable)
        {
            IsApiAvailable = isAvailable;
        }
    }
}