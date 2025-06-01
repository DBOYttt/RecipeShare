using ReciptShare.Views;

namespace ReciptShare;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register additional routes
        Routing.RegisterRoute("recipedetail", typeof(RecipeDetailPage));
        Routing.RegisterRoute("addrecipe", typeof(AddRecipePage));
        Routing.RegisterRoute("editprofile", typeof(EditProfilePage));
    }
}