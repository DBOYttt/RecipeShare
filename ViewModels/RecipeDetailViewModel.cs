using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;

namespace RecipeShare.ViewModels
{
  [QueryProperty(nameof(Title), "title")]
  public class RecipeDetailViewModel : INotifyPropertyChanged
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
          Description = $"Szczegóły przepisu: {title}";
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

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}
