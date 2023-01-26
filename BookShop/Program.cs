using BookShop.Data.Helpers;
using BookShop.Core.Data;
using BookShop.Data.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookShop.Core.Entities;

namespace BookShop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<BookDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    builder =>
                    {
                        builder.MigrationsAssembly(nameof(BookShop));
                    });

            });

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = false;
                

            }).AddEntityFrameworkStores<BookDbContext>().AddDefaultTokenProviders();

            builder.Services.Configure<AdminUser>(builder.Configuration.GetSection("AdminUser"));

            Constants.RootPath = builder.Environment.WebRootPath;
            Constants.AuthorPath = Path.Combine(Constants.RootPath, "assets", "images", "author");
            Constants.BookPath = Path.Combine(Constants.RootPath, "assets", "images", "books");
            Constants.SliderPath = Path.Combine(Constants.RootPath, "assets", "images", "sliders");
            Constants.PartnerPath = Path.Combine(Constants.RootPath, "assets", "images", "partner");
            Constants.BlogPath = Path.Combine(Constants.RootPath, "assets", "images", "blog");
            Constants.FooterLogoPath = Path.Combine(Constants.RootPath, "assets", "images", "footerLogo");
            Constants.UserPath = Path.Combine(Constants.RootPath, "assets", "images", "users");




            var app = builder.Build();

            


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var dataInitializer = new DataInitializer(serviceProvider);

                await dataInitializer.SeedData();
            }
            //using (var scope = app.Services.CreateScope(serviceProvider))
            //{

            //    var serviceProvider = scope.ServiceProvider;
            //    var dataInitializer = new DataInitializer(serviceProvider);

            //    await dataInitializer.SeedData();
            //    //var bookDbContext = scope.ServiceProvider.GetRequiredService<BookDbContext>();
            //    //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            //    //var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //    //var dataInitializer = new DataInitializer(bookDbContext, userManager, roleManager);
            //    //await dataInitializer.SeedData();
            //}

            app.UseRouting();

            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}"
                );


            });
             await app.RunAsync();
        }
    }
}