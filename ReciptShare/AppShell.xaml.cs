namespace ReciptShare;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for navigation
        Routing.RegisterRoute("recipedetail", typeof(Views.RecipeDetailPage));
        Routing.RegisterRoute("addrecipe", typeof(Views.AddRecipePage));
        Routing.RegisterRoute("editprofile", typeof(Views.EditProfilePage));
    }
}