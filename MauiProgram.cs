using Microsoft.Extensions.DependencyInjection;
using RecipeShare.Services;
using RecipeShare.Data;

namespace RecipeShare;

public static class MauiProgram
{
  public static IServiceProvider Services { get; private set; }

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

    // Rejestracja usług w DI
    builder.Services.AddSingleton<UserSessionService>();
    builder.Services.AddDbContext<AppDbContext>();

    var app = builder.Build();
    Services = app.Services;  // Zapisujemy referencję do DI
    return app;
  }
}
