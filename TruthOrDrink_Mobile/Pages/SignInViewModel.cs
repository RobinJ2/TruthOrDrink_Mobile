using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthOrDrink_Mobile.Pages
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty] // MVVM toolkit
        private string _email;

        [ObservableProperty]
        private string _password;

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
        private async Task SignIn()
        {
            try
            {
                await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);

                if (_authClient.User != null)
                {
                    await Shell.Current.GoToAsync(nameof(HomePage));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Login Fout", $"Er is iets misgegaan: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task NavigateSignUp()
        {
            await Shell.Current.GoToAsync(nameof(SignUpView));
        }
    }
}
