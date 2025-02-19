using Microsoft.EntityFrameworkCore;
using RecipeShare.Models;

namespace RecipeShare.Data
{
  public class AppDbContext : DbContext
  {
    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var connectionString = "server=10.0.2.2;port=3306;database=recipes_db;uid=root;pwd=;";
      optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
  }
}
