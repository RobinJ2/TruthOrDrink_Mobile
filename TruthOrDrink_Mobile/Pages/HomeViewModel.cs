using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Microsoft.Maui.Devices.Sensors;

namespace TruthOrDrink_Mobile.Pages
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty]
        private string _welcomeMessage;

        [ObservableProperty]
        private string _geoLocation;

        public HomeViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;

            var currentUser = _authClient.User;
            WelcomeMessage = currentUser != null && !string.IsNullOrEmpty(currentUser.Info.DisplayName)
                ? $"Welkom, {currentUser.Info.DisplayName}!"
                : "Welkom, gebruiker!";

            // Haal de geolocatie op bij het laden
            Task.Run(async () => await GetGeoLocationAsync());
        }

        public void UpdateWelcomeMessage()
        {
            var currentUser = _authClient.User;
            WelcomeMessage = currentUser != null && !string.IsNullOrEmpty(currentUser.Info.DisplayName)
                ? $"Welkom, {currentUser.Info.DisplayName}!"
                : "Welkom, gebruiker!";
        }

        [RelayCommand]
        private async Task Logout()
        {
            _authClient.SignOut();

            // Reset de WelcomeMessage
            WelcomeMessage = "Welkom, gebruiker!";

            // Ga terug naar de inlogpagina
            await Shell.Current.GoToAsync("//SignIn");
        }

        [RelayCommand]
        public async Task StartGame()
        {
            // Navigeer naar de SpelStartenPage
            await Shell.Current.GoToAsync(nameof(StartGamePage));
        }

        private async Task GetGeoLocationAsync()
        {
            try
            {
                // Zorg ervoor dat toestemming op de hoofdthread wordt gevraagd
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(10)
                    });

                    if (location != null)
                    {
                        GeoLocation = $"Lat: {location.Latitude}, Long: {location.Longitude}";
                    }
                    else
                    {
                        GeoLocation = "Locatie niet beschikbaar.";
                    }
                });
            }
            catch (Exception ex)
            {
                GeoLocation = $"Fout bij ophalen locatie: {ex.Message}";
            }
        }
    }
}