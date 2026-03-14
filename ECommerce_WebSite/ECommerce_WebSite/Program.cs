using ECommerce_WebSite.Extensions;
using Entities.Data;
using Entities.Models;
using Entities.Repos;
using Entities.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_WebSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ECommContext>(s =>
            {
                s.UseSqlServer(builder.Configuration.GetConnectionString("con1"));
            });
            builder.Services.AddIdentity<App_User, IdentityRole>(s =>
            {
                s.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<ECommContext>();
            // Register the repositories
            builder.Services.AddScoped(typeof(IEntityRepo<>), typeof(EntityRepo<>)); // If using Generic Repo
            // Register the Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // MUST be added before builder.Build()
            builder.Services.AddSession();

            var app = builder.Build();

            // ======================================================
            // SEED ROLES AND ADMIN USER
            // ======================================================
            // We must create a scope to resolve scoped services like UserManager
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Call our seeder method and wait for it to finish
                    DbSeeder.SeedRolesAndAdminAsync(services).Wait();
                }
                catch (Exception ex)
                {
                    // Log errors if needed
                    Console.WriteLine("An error occurred while seeding the database: " + ex.Message);
                }
            }
            // ======================================================

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Set the default culture of the application to Egypt (ar-EG)
            // This will automatically make .ToString("C") output Egyptian Pounds
            var supportedCultures = new[] { "ar-EG" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();   // Enable Session cookies
            app.UseAuthentication();
            app.UseAuthorization();

            // 1. Map the Area route FIRST
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // 2. Default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Catalog}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
