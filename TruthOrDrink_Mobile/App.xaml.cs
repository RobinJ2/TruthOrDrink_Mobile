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
                // Controleer of de gebruiker daadwerkelijk is ingelogd
                var currentUser = _authClient.User;

                if (currentUser != null && currentUser.Info.IsEmailVerified) // Controleer ook op e-mailverificatie
                {
                    Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
                    (MainPage as AppShell)?.UpdateShellItems(true);
                    await Shell.Current.GoToAsync("HomePage");
                }
                else
                {
                    Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
                    (MainPage as AppShell)?.UpdateShellItems(false);
                    await Shell.Current.GoToAsync("SignIn");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WTF] Fout tijdens navigatie: {ex.Message}");
            }
        }
    }
}