using Firebase.Auth;
using TruthOrDrink_Mobile.Pages;

namespace TruthOrDrink_Mobile
{
    public partial class App : Application
    {
        private readonly FirebaseAuthClient _authClient;

        public App(AppShell appShell, FirebaseAuthClient authClient)
        {
            InitializeComponent();

            _authClient = authClient;

            // Stel MainPage in voordat je Shell.Current gebruikt
            MainPage = appShell;

            // Controleer de loginstatus na het instellen van MainPage
            if (_authClient.User != null)
            {
                appShell.SetShellItemsVisibility(true);
                Shell.Current.GoToAsync(nameof(HomePage));
            }
            else
            {
                appShell.SetShellItemsVisibility(false);
                //Shell.Current.GoToAsync(nameof(SignInView));
            }
        }
    }
}