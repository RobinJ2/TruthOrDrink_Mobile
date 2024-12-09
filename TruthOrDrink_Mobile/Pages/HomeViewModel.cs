using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace TruthOrDrink_Mobile.Pages
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty]
        private string _welcomeMessage;

        public HomeViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;

            var currentUser = _authClient.User;
            WelcomeMessage = currentUser != null
                ? $"Welkom, {currentUser.Info.DisplayName}!"
                : "Welkom, gebruiker!";
        }

        [RelayCommand]
        private async Task Logout()
        {
            _authClient.SignOut();

            // Ga terug naar de inlogpagina
            await Shell.Current.GoToAsync("//SignIn");
        }
    }
}