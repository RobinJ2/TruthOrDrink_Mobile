using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace TruthOrDrink_Mobile
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Zorg ervoor dat de content zich uitstrekt onder de status- en navigatiebalk
            Window.SetDecorFitsSystemWindows(false);

            // Maak statusbalk en navigatiebalk doorzichtig
            Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            Window.SetNavigationBarColor(Android.Graphics.Color.Transparent);
        }
    }
}
