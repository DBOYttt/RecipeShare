using RecipeShare.Models;

namespace RecipeShare.Services
{
  public class UserSessionService
  {
    public User CurrentUser { get; set; }
    public bool IsLoggedIn => CurrentUser != null;
  }
}
