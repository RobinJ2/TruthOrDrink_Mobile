using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

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
                Shell.Current.GoToAsync("HomePage");
            }
        }

        [RelayCommand]
        public async Task SignIn()
        {
            try
            {
                var user = await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);

                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
                (Application.Current.MainPage as AppShell)?.UpdateShellItems(true);

                await Shell.Current.GoToAsync("HomePage");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task NavigateSignUp()
        {
            // Navigeer naar SignUpView
            await Shell.Current.GoToAsync(nameof(SignUpView));
        }
    }
}
