using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using System.Text.RegularExpressions;

namespace TruthOrDrink_Mobile.Pages
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty] // MVVM toolkit
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _emailError; // Voor validatiebericht

        [ObservableProperty]
        private string _passwordError; // Voor validatiebericht

        public string Username => _authClient.User?.Info?.DisplayName;

        public SignInViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;

            // Controleer of de gebruiker al ingelogd is
            if (_authClient.User != null)
            {
                // Gebruiker is al ingelogd, navigeren naar HomePage
                Shell.Current.GoToAsync("//HomePage");
            }
        }

        [RelayCommand]
        public async Task SignIn()
        {
            // Valideer invoer
            if (!ValidateInputs())
            
                return;
            

            try
            {
                var user = await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);

                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
                (Application.Current.MainPage as AppShell)?.UpdateShellItems(true);

                // Update de WelcomeMessage
                var homeViewModel = Application.Current.MainPage.BindingContext as HomeViewModel;
                homeViewModel?.UpdateWelcomeMessage();

                await Shell.Current.GoToAsync("//HomePage");
            }
            catch (Exception ex)
            {
                // Toon foutbericht als inloggen mislukt
                //await Application.Current.MainPage.DisplayAlert("Error", "E-mail of wachtwoord is onjuist.", "OK");
                PasswordError = "Wachtwoord is onjuist.";
                EmailError = "E-mail of wachtwoord is onjuist.";
            }
        }

        [RelayCommand]
        private async Task NavigateSignUp()
        {
            // Navigeer naar SignUpView
            await Shell.Current.GoToAsync(nameof(SignUpView));
        }

        private bool ValidateInputs()
        {
            bool isValid = true;

            // Validatie voor e-mail
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "E-mail mag niet leeg zijn.";
                isValid = false;
            }
            else if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                EmailError = "E-mail is niet geldig.";
                isValid = false;
            }
            else
            {
                EmailError = null;
            }

            // Validatie voor wachtwoord
            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Wachtwoord mag niet leeg zijn.";
                isValid = false;
            }
            else
            {
                PasswordError = null;
            }

            return isValid;
        }
    }
}