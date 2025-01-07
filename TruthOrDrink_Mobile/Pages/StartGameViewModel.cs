using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthOrDrink_Mobile.Pages
{
    public partial class StartGameViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isCustomQuestionsEnabled;

        public StartGameViewModel()
        {
            IsCustomQuestionsEnabled = false; // Standaard niet geselecteerd
        }

        [RelayCommand]
        private void ToggleCustomQuestions()
        {
            IsCustomQuestionsEnabled = !IsCustomQuestionsEnabled;
        }
    
}
}
