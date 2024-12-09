using Firebase.Auth;

namespace TruthOrDrink_Mobile
{
    public partial class App : Application
    {
        private readonly FirebaseAuthClient _authClient;

        public App(AppShell appShell, FirebaseAuthClient authClient)
        {
            InitializeComponent();
            _authClient = authClient;

            // Instellen van de startpagina van de app
            MainPage = appShell;
            NavigateToInitialPage();
        }

        private async void NavigateToInitialPage()
        {
            try
            {
                var currentUser = _authClient?.User;

                if (currentUser == null || !currentUser.Info?.IsEmailVerified == true)
                {
                    // Geen gebruiker: ga naar de inlogpagina
                    await Shell.Current.GoToAsync("SignIn");
                }
                else
                {
                    // Ingelogde gebruiker: ga naar de tabs (homepagina)
                    await Shell.Current.GoToAsync("HomePage");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Fout tijdens navigatie: {ex.Message}");
            }
        }
    }
}