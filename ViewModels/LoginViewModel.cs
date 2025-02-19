using System.Windows.Input;
using Microsoft.Maui.Controls;
using RecipeShare.Data;
using RecipeShare.Models;
using RecipeShare.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; // dla GetService<T>()

namespace RecipeShare.ViewModels
{
  public class LoginViewModel : BaseViewModel
  {
    private string email;
    private string password;
    private string message;

    public string Email
    {
      get => email;
      set => SetProperty(ref email, value);
    }
    public string Password
    {
      get => password;
      set => SetProperty(ref password, value);
    }
    public string Message
    {
      get => message;
      set => SetProperty(ref message, value);
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel()
    {
      LoginCommand = new Command(OnLogin);
    }

    private async void OnLogin()
    {
      try
      {
        using (var db = new AppDbContext())
        {
          var user = await db.Users.FirstOrDefaultAsync(u => u.Email == Email && u.PasswordHash == Password);
          if (user != null)
          {
            // Pobieramy usługę z DI poprzez statyczną właściwość MauiProgram.Services
            var session = MauiProgram.Services.GetService<UserSessionService>();
            if (session != null)
            {
              session.CurrentUser = user;
            }
            Application.Current.MainPage = new AppShell();
          }
          else
          {
            Message = "Błędne dane logowania";
          }
        }
      }
      catch (Exception ex)
      {
        Message = "Błąd logowania: " + ex.Message;
      }
    }
  }
}
