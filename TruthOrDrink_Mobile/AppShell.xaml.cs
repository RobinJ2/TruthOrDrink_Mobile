using TruthOrDrink_Mobile.Pages;

namespace TruthOrDrink_Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell(HomeViewModel homeViewModel)
        {
            InitializeComponent();
            BindingContext = homeViewModel;

            // Registreer routes
            Routing.RegisterRoute("HomePage", typeof(Pages.HomePage));
            Routing.RegisterRoute("SignInView", typeof(Pages.SignInView));
            Routing.RegisterRoute("SignUpView", typeof(Pages.SignUpView));
        }

        public void SetShellItemsVisibility(bool isAuthenticated)
        {
            // Stel zichtbaarheid van Shell-items in op basis van authenticatiestatus
            FlyoutBehavior = isAuthenticated ? FlyoutBehavior.Flyout : FlyoutBehavior.Disabled;

              foreach (var item in Items)
            {
                if (item.Route == nameof(HomePage))
                    item.IsVisible = isAuthenticated;

                if (item.Route == nameof(SignInView) || item.Route == nameof(SignUpView))
                    item.IsVisible = !isAuthenticated;
            }
        }

        public void UpdateShellItems(bool isAuthenticated)
        {
            Console.WriteLine($"[WTF] UpdateShellItems aangeroepen: {isAuthenticated}");
            

            // Verander de zichtbaarheid van items
            var homePageShellContent = this.FindByName<ShellContent>("homePageShellContent");
            if (homePageShellContent != null)
            {
                Console.WriteLine("[WTF] homePageShellContent gevonden.");
                homePageShellContent.IsVisible = isAuthenticated;
            }
            else
            {
                Console.WriteLine("[WTF] homePageShellContent niet gevonden! Controleer de naam in de XAML.");
            }

            Console.WriteLine("[WTF] Controleer Items in AppShell:");
            foreach (var item in Items)
            {
                Console.WriteLine($"[WTF] Item: {item.Route}, IsVisible: {item.IsVisible}");
            }

            FlyoutBehavior = isAuthenticated ? FlyoutBehavior.Flyout : FlyoutBehavior.Disabled;

        }
    }
}