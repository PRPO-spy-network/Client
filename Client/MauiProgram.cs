using Client.Pages;
using Client.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();

			#region api
			using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Client.appsettings.json");

			IConfiguration config = new ConfigurationBuilder()
			.AddJsonStream(stream!)
			.Build();

			builder.Services.AddHttpClient();
			#endregion


			builder
				.UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
			
            builder.Services.AddSingleton<IConfiguration>(config);

			builder.Services.AddTransient<LocationsViewModel>();
			builder.Services.AddTransient<LocationsPage>();

			builder.Services.AddTransient<RegistrationsViewModel>();
			builder.Services.AddTransient<RegistrationsPage>();

#if DEBUG
			builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
