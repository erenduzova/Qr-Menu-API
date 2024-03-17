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
            builder.Entity<ApplicationUser>().HasOne(au => au.State).WithMany(s => s.ApplicationUsers).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Company>().HasOne(c => c.State).WithMany(s => s.Companies).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Restaurant>().HasOne(r => r.State).WithMany(s => s.Restaurants).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Category>().HasOne(c => c.State).WithMany(s => s.Categories).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Food>().HasOne(f => f.State).WithMany(s => s.Foods).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RestaurantUser>().HasOne(r => r.Restaurant).WithMany().OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RestaurantUser>().HasKey(ru => new { ru.UserId, ru.RestaurantId });

            base.OnModelCreating(builder);
        }
    }
}
