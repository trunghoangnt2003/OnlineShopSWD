using CloudinaryDotNet;
using dotenv.net;
using OnlineShop.Helper;
using OnlineShop.Interfaces;
using OnlineShop.Services;

namespace OnlineShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddScoped<IPhotoService,PhotoService>();
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            var cloudinarySettings = new CloudinarySettings
            {
                CloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUDNAME"),
                ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_APIKEY"),
                ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_APISECRET")
            };
            builder.Services.Configure<CloudinarySettings>(options =>
            {
                options.CloudName = cloudinarySettings.CloudName;
                options.ApiKey = cloudinarySettings.ApiKey;
                options.ApiSecret = cloudinarySettings.ApiSecret;
            });
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
