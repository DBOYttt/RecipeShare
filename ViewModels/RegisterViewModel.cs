using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using RecipeShare.Data;
using RecipeShare.Models;
using RecipeShare.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace RecipeShare.ViewModels
{
  public class RegisterViewModel : BaseViewModel
  {
    private string email;
    private string username;
    private string password;
    private string confirmPassword;
    private string message;

    public string Email { get => email; set => SetProperty(ref email, value); }
    public string Username { get => username; set => SetProperty(ref username, value); }
    public string Password { get => password; set => SetProperty(ref password, value); }
    public string ConfirmPassword { get => confirmPassword; set => SetProperty(ref confirmPassword, value); }
    public string Message { get => message; set => SetProperty(ref message, value); }

    public ICommand RegisterCommand { get; }

    public RegisterViewModel()
    {
      RegisterCommand = new Command(OnRegister);
    }

    private async void OnRegister()
    {
      if (Password != ConfirmPassword)
      {
        Message = "Hasła nie są identyczne";
        return;
      }
      try
      {
        using (var db = new AppDbContext())
        {
          var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == Email);
          if (existingUser != null)
          {
            Message = "Użytkownik z tym emailem już istnieje";
            return;
          }
          var newUser = new User
          {
            Email = Email,
            Username = Username,
            PasswordHash = Password, // W produkcji należy zahashować hasło
            RegistrationDate = DateTime.Now
          };
          db.Users.Add(newUser);
          await db.SaveChangesAsync();

          Message = "Rejestracja zakończona sukcesem";

          // Spróbuj pobrać usługę UserSessionService z naszej statycznej referencji
          var session = MauiProgram.Services?.GetService<UserSessionService>();
          if (session == null)
          {
            // Jeśli dalej null, spróbuj przez Application.Current.Handler
            session = (Application.Current as App)?.Handler?.MauiContext?.Services.GetService<UserSessionService>();
          }

          if (session == null)
          {
            Debug.WriteLine("UserSessionService jest nadal null!");
          }
          else
          {
            session.CurrentUser = newUser;
          }

          // Upewnij się, że Application.Current nie jest null
          if (Application.Current == null)
          {
            Debug.WriteLine("Application.Current jest null!");
          }

          // Nawigacja do głównej strony
          Application.Current.MainPage = new AppShell();
        }
      }
      catch (Exception ex)
      {
        Message = "Błąd rejestracji: " + ex.Message;
        Debug.WriteLine("Błąd rejestracji: " + ex);
      }
    }
  }
}
