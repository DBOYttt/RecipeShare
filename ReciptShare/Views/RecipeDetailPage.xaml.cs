using ReciptShare.ViewModels;

namespace ReciptShare.Views;

public partial class RecipeDetailPage : ContentPage
{
    private ReciptDetailViewModel _viewModel;

    public RecipeDetailPage(ReciptDetailViewModel viewModel)
    {
        try
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
        catch (Exception ex)
        {
            // Handle initialization error
            DisplayAlert("Error", $"Failed to initialize page: {ex.Message}", "OK");
        }
    }

    private void OnStarClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && int.TryParse(button.ClassId, out int rating))
            {
                _viewModel.UserRating = rating;
                
                // Update star appearance
                var parent = button.Parent as StackLayout;
                if (parent != null)
                {
                    for (int i = 0; i < parent.Children.Count; i++)
                    {
                        if (parent.Children[i] is Button star)
                        {
                            star.Text = i < rating ? "⭐" : "☆";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Failed to set rating: {ex.Message}", "OK");
        }
    }
}