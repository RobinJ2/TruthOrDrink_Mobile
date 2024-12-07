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
            try
            {
                _authClient.SignOut();

                // Navigeren naar de loginpagina
                var appShell = (AppShell)Shell.Current;
                appShell.SetShellItemsVisibility(false);

                await Shell.Current.GoToAsync(nameof(SignInView));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Er is iets misgegaan tijdens het uitloggen: {ex.Message}", "OK");
            }
        }
    }
}