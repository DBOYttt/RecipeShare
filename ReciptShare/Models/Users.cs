namespace ReciptShare.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string ProfileImageUrl { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
    public string Bio { get; set; } = string.Empty;
    public int RecipesCount { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
}