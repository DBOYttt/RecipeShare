using Microsoft.Extensions.Logging;
using ReciptShare.ViewModels;
using ReciptShare.Views;
using ReciptShare.Services;

namespace ReciptShare;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register services
        builder.Services.AddSingleton<IHttpClientService, HttpClientService>();
        builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
        builder.Services.AddSingleton<IApiStatusService, ApiStatusService>();
        builder.Services.AddSingleton<MockDataService>();
        
        // Register ViewModels
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<BrowseViewModel>();
        builder.Services.AddTransient<FavoritesViewModel>();
        builder.Services.AddTransient<ShoppingListViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<ReciptDetailViewModel>();
        builder.Services.AddTransient<AddReciptViewModel>();
        builder.Services.AddTransient<EditProfileViewModel>();
        
        // Register Views
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<BrowsePage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<ShoppingListPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<RecipeDetailPage>();
        builder.Services.AddTransient<AddRecipePage>();
        builder.Services.AddTransient<EditProfilePage>();

        builder.Logging.AddDebug();

        return builder.Build();
    }
}