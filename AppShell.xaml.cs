namespace RecipeShare;

public partial class AppShell : Shell
{
  public AppShell()
  {
    InitializeComponent();
    Routing.RegisterRoute(nameof(RecipeDetailPage), typeof(RecipeDetailPage));
    Routing.RegisterRoute(nameof(FavoritesPage), typeof(FavoritesPage));
  }
}

