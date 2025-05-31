namespace ReciptShare.Models;

public class Rating
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Stars { get; set; } // 1-5
    public string Review { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}