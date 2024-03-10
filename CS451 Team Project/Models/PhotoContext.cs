using Microsoft.EntityFrameworkCore;
using CS451_Team_Project.Models;

public class PhotoAppContext : DbContext
{
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Photo> Photos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=photo_app;Trusted_Connection=True;");
    }
}
