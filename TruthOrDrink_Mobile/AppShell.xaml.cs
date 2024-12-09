using TruthOrDrink_Mobile.Pages;

namespace TruthOrDrink_Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell(HomeViewModel homeViewModel)
        {

            InitializeComponent();
            BindingContext = homeViewModel;

            Routing.RegisterRoute("HomePage", typeof(Pages.HomePage));
            Routing.RegisterRoute("SignInView", typeof(Pages.SignInView));
            Routing.RegisterRoute("SignUpView", typeof(Pages.SignUpView));
            Routing.RegisterRoute("ManageQuestionsPage", typeof(Pages.ManageQuestionsPage));
            Routing.RegisterRoute("SettingsPage", typeof(Pages.SettingsPage));
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
            // Verander de zichtbaarheid van items
            var homePageShellContent = this.FindByName<ShellContent>("homePageShellContent");
            if (homePageShellContent != null)
            {
                homePageShellContent.IsVisible = isAuthenticated;
            }

        }
    }
}