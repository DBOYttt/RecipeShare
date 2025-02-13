using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RecipeShare
{
  public class Recipe : INotifyPropertyChanged
  {
    private bool isFavorite;

    public string Title { get; set; }
    public string Description { get; set; }

    public bool IsFavorite
    {
      get => isFavorite;
      set
      {
        if (isFavorite != value)
        {
          isFavorite = value;
          OnPropertyChanged();
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}
