using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using IT_13FinalProject.Services;

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
            
            // Add services
            builder.Services.AddScoped<IUserAccountService, InMemoryUserAccountService>();
            builder.Services.AddSingleton<IHealthRecordService, InMemoryHealthRecordService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            return app;
        }
    }
}
