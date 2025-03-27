using Microsoft.EntityFrameworkCore;
namespace macro_tracker_web_service.Models
{
    public class MacroTrackerContext : DbContext
    {
        public MacroTrackerContext(DbContextOptions<MacroTrackerContext> options) : base(options) { }

        public DbSet<FoodPer100g> FoodsPer100g { get; set; } 
        public DbSet<Users> Users { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodPer100g>().ToTable("FoodPer100g"); 
            modelBuilder.Entity<Users>().ToTable("Users"); 
        }
    }
}