using Microsoft.AspNetCore.Authentication.Cookies;
using TravelExpertsDB;
using Microsoft.EntityFrameworkCore;

namespace TravelExpertsMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {   
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(); // Add session services
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(opt => {
                opt.LoginPath = "/Account/Login";
                opt.ExpireTimeSpan = TimeSpan.FromHours(1);
                opt.SlidingExpiration = true;
            }) ; // Add more authentication schemes if needed

            builder.Services.AddDbContext<TravelExpertsContext>
                (options => options.UseSqlServer(builder.Configuration.GetConnectionString("travelexpertscon")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStatusCodePages(); // for more user friendly error pages for 404 and 403 errors
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); //added

            app.UseAuthorization();
            app.UseSession(); // needed to use session state (Must be before UseEndpoints).

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
