using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using IT_13FinalProject.Services;
using IT_13FinalProject.Data;

namespace IT_13FinalProject
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            
            // Add configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            builder.Configuration.AddConfiguration(configuration);
            
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            
            // Add database context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            // Add services
            builder.Services.AddScoped<IUserAccountService, DatabaseUserAccountService>();
            builder.Services.AddSingleton<IHealthRecordService, InMemoryHealthRecordService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Initialize database in background thread to avoid blocking UI
            Task.Run(() => 
            {
                try
                {
                    InitializeDatabase(app);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Database initialization failed: {ex.Message}");
                }
            });

            return app;
        }

        private static void InitializeDatabase(MauiApp app)
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                DatabaseInitializer.InitializeAsync(context).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization failed: {ex.Message}");
                // Don't throw - let the app continue, but database operations may fail
            }
        }
    }
}
