namespace WeatherForecastProject
{
    using FluentValidation;

    using WeatherForecastProject.Models;
    using WeatherForecastProject.Validators;

    public static class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services
            .AddControllersWithViews()
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            
            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IValidator<WeatherForecastModel>, WeatherForecastValidator>();
            builder.Services.AddScoped<IValidator<DailyForecast>, DailyForecastValidator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "weatherforecast",
                pattern: "{controller=WeatherForecast}/{action=Index}/{id?}");

            app.Run();
        }
    }
}