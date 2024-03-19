
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Data;
using Qr_Menu_API.Models;
using Qr_Menu_API.Services;

namespace Qr_Menu_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationContext")));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            builder.Services.AddScoped<CompaniesService>();
            builder.Services.AddScoped<RestaurantsService>();
            builder.Services.AddScoped<CategoriesService>();
            builder.Services.AddScoped<FoodsService>();
            builder.Services.AddScoped<RolesService>();
            builder.Services.AddScoped<UsersService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            ApplicationContext applicationContext = app.Services.CreateScope().ServiceProvider.GetService<ApplicationContext>()!;
            UserManager<ApplicationUser>? userManager = app.Services.CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole>? roleManager = app.Services.CreateScope().ServiceProvider.GetService<RoleManager<IdentityRole>>();
            if (roleManager != null)
            {
                if (userManager != null)
                {
                    DbInitializer dbInitializer = new DbInitializer(applicationContext, userManager, roleManager);
                    dbInitializer.Initialize();
                }

            }
            app.Run();
        }
    }
}
