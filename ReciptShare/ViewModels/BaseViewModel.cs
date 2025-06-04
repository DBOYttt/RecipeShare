using CommunityToolkit.Mvvm.ComponentModel;

namespace ReciptShare.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        bool isBusy;

        [ObservableProperty]
        string title = string.Empty;

        [ObservableProperty]
        bool isApiConnected = true;

        public BaseViewModel()
        {
            // Keep it simple for now - we'll handle API status in individual ViewModels
        }

        protected void SetApiStatus(bool isConnected)
        {
            IsApiConnected = isConnected;
        }
    }
}