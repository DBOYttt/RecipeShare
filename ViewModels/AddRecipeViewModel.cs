using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace RecipeShare.ViewModels
{
  public class AddRecipeViewModel : INotifyPropertyChanged
  {
    private string title;
    private string description;

    public string Title
    {
      get => title;
      set
      {
        if (title != value)
        {
          title = value;
          OnPropertyChanged();
        }
      }
    }

    public string Description
    {
      get => description;
      set
      {
        if (description != value)
        {
          description = value;
          OnPropertyChanged();
        }
      }
    }

    public ICommand AddRecipeCommand { get; }

    public AddRecipeViewModel()
    {
      AddRecipeCommand = new Command(OnAddRecipe);
    }

    private async void OnAddRecipe()
    {
      await Application.Current.MainPage.DisplayAlert("Przepis dodany", "Twój przepis został dodany!", "OK");

      Title = string.Empty;
      Description = string.Empty;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}
