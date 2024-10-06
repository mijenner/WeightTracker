using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using WeightTracker.Services;
using WeightTracker.ViewModels;
using WeightTracker.Views;

namespace WeightTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Application specific services and view-models - singleton (en instans) 
            // builder.Services.AddSingleton<IStorageService, InMemoryStorage>();
            builder.Services.AddSingleton<IStorageService, JSONFileStorage>();
            builder.Services.AddSingleton<IUserProfileService, UserProfileService>();
            builder.Services.AddSingleton<MeasurementsViewModel>();
            builder.Services.AddSingleton<UserProfileViewModel>();
            // App specific pages
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<EditProfilePage>(); 

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
