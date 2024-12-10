using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TruthOrDrink_Mobile.Pages
{
    public class SettingsPageViewModel
    {
        public ICommand LogoutCommand { get; }

        public SettingsPageViewModel()
        {
            LogoutCommand = new Command(OnLogout);
        }


        private void OnLogout()
        {
            // Logica om uit te loggen
            Shell.Current.GoToAsync("//SignIn");
        }
    }
}
