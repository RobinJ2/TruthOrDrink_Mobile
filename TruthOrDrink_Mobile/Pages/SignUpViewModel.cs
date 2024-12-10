using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace TruthOrDrink_Mobile.Pages
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty] // MVVM toolkit
        private string _email;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _confirmPassword;

        [ObservableProperty]
        private string _emailError;

        [ObservableProperty]
        private string _usernameError;

        [ObservableProperty]
        private string _passwordError;

        [ObservableProperty]
        private string _confirmPasswordError;

        public SignUpViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task SignUp()
        {
            // Controleer of de invoer geldig is
            if (!ValidateInputs())
                return;

            try
            {
                // Maak een gebruiker aan met Firebase
                await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password, Username);

                // Succesvolle registratie, navigeer naar de inlogpagina
                await Shell.Current.GoToAsync("//SignIn");
            }
            catch (FirebaseAuthException ex)
            {
                // Specifieke foutmeldingen van Firebase
                if (ex.Reason == AuthErrorReason.EmailExists)
                {
                    EmailError = "Dit e-mailadres is al in gebruik.";
                }
                else if (ex.Reason == AuthErrorReason.InvalidEmailAddress)
                {
                    EmailError = "Ongeldig e-mailadres.";
                }
                else if (ex.Reason == AuthErrorReason.WeakPassword)
                {
                    PasswordError = "Wachtwoord is te zwak.";
                }
                else
                {
                    // Algemene foutmelding
                    Console.WriteLine($"Fout tijdens registratie: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // Foutmelding voor andere problemen
                Console.WriteLine($"Onverwachte fout tijdens registratie: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task NavigateSignIn()
        {
            await Shell.Current.GoToAsync("//SignIn");
        }

        private bool ValidateInputs()
        {
            bool isValid = true;

            // Validate email
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

            // Validate username
            if (string.IsNullOrWhiteSpace(Username))
            {
                UsernameError = "Gebruikersnaam mag niet leeg zijn.";
                isValid = false;
            }
            else
            {
                UsernameError = null;
            }

            // Validate password
            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Wachtwoord mag niet leeg zijn.";
                isValid = false;
            }
            else if (Password.Length < 6 || !Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$"))
            {
                PasswordError = "Wachtwoord moet minstens 6 tekens, 1 hoofdletter en 1 cijfer bevatten.";
                isValid = false;
            }
            else
            {
                PasswordError = null;
            }

            // Validate confirm password
            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ConfirmPasswordError = "Bevestig je wachtwoord.";
                isValid = false;
            }
            else if (ConfirmPassword != Password)
            {
                ConfirmPasswordError = "Wachtwoorden komen niet overeen.";
                isValid = false;
            }
            else
            {
                ConfirmPasswordError = null;
            }

            return isValid;
        }
    }
}
