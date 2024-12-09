using Firebase.Auth;

namespace TruthOrDrink_Mobile
{
    public partial class App : Application
    {
        private readonly FirebaseAuthClient _authClient;
        private readonly IServiceProvider _serviceProvider;

        public App(FirebaseAuthClient authClient, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _authClient = authClient;
            _serviceProvider = serviceProvider;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var appShell = _serviceProvider.GetRequiredService<AppShell>();

            var currentUser = _authClient?.User;

            // Controleer de inlogstatus en stel de juiste startpagina in
            if (currentUser == null || !currentUser.Info?.IsEmailVerified == true)
            {
                appShell.GoToAsync("//SignIn");
            }
            else
            {
                appShell.GoToAsync("//HomePage");
            }

            return new Window(appShell);
        }
    }
}
