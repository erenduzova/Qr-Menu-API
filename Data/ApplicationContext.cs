using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Models;
using System.Reflection.Emit;

namespace Qr_Menu_API.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<State>? States { get; set; }
        public DbSet<Company>? Companies { get; set; }
        public DbSet<Restaurant>? Restaurants { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Food>? Foods { get; set; }
        public DbSet<RestaurantUser>? RestaurantUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // These entities has one-to-many relation with State entity.
            // Restrict deletion of the State when others deleted.
            builder.Entity<ApplicationUser>().HasOne(au => au.State).WithMany(s => s.ApplicationUsers).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Company>().HasOne(c => c.State).WithMany(s => s.Companies).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Restaurant>().HasOne(r => r.State).WithMany(s => s.Restaurants).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Category>().HasOne(c => c.State).WithMany(s => s.Categories).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Food>().HasOne(f => f.State).WithMany(s => s.Foods).OnDelete(DeleteBehavior.Restrict);

            // RestaurantUser shows many-to-many relationship between restaurant and ApplicationUser.
            // No Action when RestaurantUser deleted.
            builder.Entity<RestaurantUser>().HasKey(ru => new { ru.UserId, ru.RestaurantId });
            builder.Entity<RestaurantUser>()
                .HasOne(ru => ru.Restaurant)
                .WithMany(r => r.RestaurantUsers)
                .HasForeignKey(ru => ru.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<RestaurantUser>()
                .HasOne(ru => ru.ApplicationUser)
                .WithMany(u => u.RestaurantUsers)
                .HasForeignKey(ru => ru.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }
    }
}
