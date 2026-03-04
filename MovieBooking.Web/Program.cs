using Microsoft.AspNetCore.Authentication.Cookies;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.Services;
using System.Text.Json.Serialization;
namespace MovieBooking.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient<IAuthenticationMvcService,AuthenticationMvcService>((ServiceProvider, Client) =>
            {
                var Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
                Client.BaseAddress = new Uri(Configuration["ApiSettings:BaseUrl"]!);
            });
            builder.Services.AddHttpClient<IBookingMvcService, BookingMvcService>((ServiceProvider, Client) =>
            {
                var Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
                Client.BaseAddress = new Uri(Configuration["ApiSettings:BaseUrl"]!);
            });
            builder.Services.AddHttpClient<IAdminMvcService, RequestMvcService>((ServiceProvider, Client) =>
            {
                var Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
                Client.BaseAddress = new Uri(Configuration["ApiSettings:BaseUrl"]!);
            });
            builder.Services.AddHttpClient<IAdminManagementMvcService, AdminManagementMvcService>((ServiceProvider, Client) =>
            {
                var Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
                Client.BaseAddress = new Uri(Configuration["ApiSettings:BaseUrl"]!);
            });
            builder.Services.AddHttpClient<IMovieMvcService, MovieMvcService>((ServiceProvider, Client) =>
            {
                var Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
                Client.BaseAddress = new Uri(Configuration["ApiSettings:BaseUrl"]!);
            });
            builder.Services.AddHttpClient<ITheatreMvcService,TheatreMvcService>((ServiceProvider, Client) =>
            {
                var Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
                Client.BaseAddress = new Uri(Configuration["ApiSettings:BaseUrl"]!);
            });
            builder.Services.AddHttpClient<IScreenMvcService, ScreenMvcService>((ServiceProvider, Client) =>
            {
                var Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
                Client.BaseAddress = new Uri(Configuration["ApiSettings:BaseUrl"]!);
            });
            builder.Services.AddHttpClient<IShowTimeMvcService, ShowTimeMvcService>((ServiceProvider, Client) =>
            {
                var Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
                Client.BaseAddress = new Uri(Configuration["ApiSettings:BaseUrl"]!);
            });
            builder.Services.AddHttpClient<ILanguageMvcService, LanguageMvcService>((ServiceProvider, Client) =>
            {
                var Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
                Client.BaseAddress = new Uri(Configuration["ApiSettings:BaseUrl"]!);
            });
            builder.Services.AddHttpClient<IRequestApprovalMvcService, RequestApprovalMvcService>((ServiceProvider, Client) =>
            {
                var Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
                Client.BaseAddress = new Uri(Configuration["ApiSettings:BaseUrl"]!);
            });
            builder.Services.AddDistributedMemoryCache();
           
            builder.Services
             .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options =>
             {
                 options.LoginPath = "/Account/Login";
                 options.AccessDeniedPath = "/Account/AccessDenied";
                 options.ExpireTimeSpan = TimeSpan.FromDays(1);
                 options.SlidingExpiration = true;
             });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthorization();
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

            app.UseRouting();
            //app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
