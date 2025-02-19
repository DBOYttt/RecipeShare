using Microsoft.Extensions.DependencyInjection;
using RecipeShare.Data;
using RecipeShare.Services;
using System.Diagnostics;

namespace RecipeShare;

public partial class App : Application
{
  public static bool IsDemoMode { get; set; } = false;

  public App()
  {
    InitializeComponent();

    try
    {
      using (var db = new AppDbContext())
      {
        if (db.Database.EnsureCreated())
        {
          Debug.WriteLine("Baza danych została utworzona, ponieważ nie istniała wcześniej.");
        }
        else
        {
          Debug.WriteLine("Baza danych już istnieje.");
        }
      }
      IsDemoMode = false;
      Debug.WriteLine("Połączenie z bazą danych nawiązane pomyślnie.");
    }
    catch (Exception ex)
    {

      IsDemoMode = true;
      Debug.WriteLine("Błąd połączenia/utworzenia bazy danych: " + ex.Message);
    }


    var session = (Application.Current as App)?.Handler?.MauiContext?.Services.GetService<UserSessionService>();

    if (session != null && session.IsLoggedIn)
    {
      Debug.WriteLine("Użytkownik zalogowany. Przechodzę do AppShell.");
      MainPage = new AppShell();
    }
    else
    {
      Debug.WriteLine("Brak zalogowanego użytkownika. Przechodzę do WelcomePage.");
      MainPage = new NavigationPage(new WelcomePage());
    }
  }
}
