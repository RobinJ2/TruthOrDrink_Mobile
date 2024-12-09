using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Microsoft.Extensions.Logging;
using TruthOrDrink_Mobile.Pages;
using CommunityToolkit.Maui;

namespace TruthOrDrink_Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            //Basisconfiguratie van de applicatie
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();

#if DEBUG
            builder.Logging.AddDebug(); // Voeg logging toe voor debuggen
#endif

            // Dependency injection voor services en viewmodels
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig() 
            { 
                ApiKey = "AIzaSyDMDXy3X-2QJUVmPw_0xmPmLhmrfMzLhBY", 
                AuthDomain = "truthordrink-5409f.firebaseapp.com", 
                Providers = new FirebaseAuthProvider[] 
                    { 
                        new EmailProvider() 
                    }, 
                UserRepository = new FileUserRepository("LogInf"), 
            }));
            builder.Services.AddSingleton<SignInView>();
            builder.Services.AddSingleton<SignInViewModel>();
            builder.Services.AddSingleton<SignUpView>();
            builder.Services.AddSingleton<SignUpViewModel>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomeViewModel>();

            return builder.Build();
        }
    }
}