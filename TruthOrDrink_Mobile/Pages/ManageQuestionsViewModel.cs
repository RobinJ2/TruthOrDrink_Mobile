using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace TruthOrDrink_Mobile.Pages
{
    public partial class ManageQuestionsViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Question> questions;

        [ObservableProperty]
        private bool isAddQuestionFormVisible;

        [ObservableProperty]
        private string newQuestionText;

        [ObservableProperty]
        private string newQuestionCategory;

        [ObservableProperty]
        private int newQuestionLevel;

        [ObservableProperty]
        private Question selectedQuestion;

        public ManageQuestionsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            IsAddQuestionFormVisible = false;
            Task.Run(async () => await LoadQuestionsAsync());
            
        }

        [RelayCommand]
        private void ShowAddQuestionForm()
        {
            // Reset formulier voor toevoegen
            SelectedQuestion = null;
            NewQuestionText = string.Empty;
            NewQuestionCategory = string.Empty;
            NewQuestionLevel = 1;
            IsAddQuestionFormVisible = true;
        }

        private async Task LoadQuestionsAsync()
        {
            try
            {
                var questionList = await _databaseService.GetQuestionsAsync();
                Console.WriteLine($"Aantal vragen opgehaald: {questionList.Count}");
                Questions = new ObservableCollection<Question>(questionList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij laden van vragen: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task AddNewQuestion()
        {
            if (string.IsNullOrWhiteSpace(NewQuestionText) || string.IsNullOrWhiteSpace(NewQuestionCategory))
            {
                Console.WriteLine("Vul alle velden in.");
                return;
            }

            var newQuestion = new Question
            {
                Text = NewQuestionText,
                Category = NewQuestionCategory,
                Level = NewQuestionLevel
            };

            try
            {
                await _databaseService.AddQuestionAsync(newQuestion);
                Questions.Add(newQuestion);

                // Reset formulier
                NewQuestionText = string.Empty;
                NewQuestionCategory = string.Empty;
                NewQuestionLevel = 1;
                IsAddQuestionFormVisible = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij toevoegen van nieuwe vraag: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task DeleteQuestion(Question question)
        {
            if (question == null) return;

            try
            {
                if (Questions.Contains(question))
                {
                    Questions.Remove(question);
                    await _databaseService.DeleteQuestionAsync(question);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij verwijderen van vraag: {ex.Message}");
            }
        }

        [RelayCommand]
        private void ShowEditQuestionForm(Question question)
        {
            // Vul het formulier met de geselecteerde vraag
            SelectedQuestion = question;
            NewQuestionText = question.Text;
            NewQuestionCategory = question.Category;
            NewQuestionLevel = question.Level;
            IsAddQuestionFormVisible = true;
        }

        [RelayCommand]
        private async Task EditQuestion(Question question)
        {
            if (question == null) return;

            try
            {
                await _databaseService.UpdateQuestionAsync(question);

                var index = Questions.IndexOf(question);
                if (index >= 0)
                {
                    Questions[index] = question;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij bewerken van vraag: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task SaveQuestion()
        {
            if (string.IsNullOrWhiteSpace(NewQuestionText) || string.IsNullOrWhiteSpace(NewQuestionCategory))
            {
                Console.WriteLine("Vul alle velden in.");
                return;
            }

            if (SelectedQuestion == null)
            {
                // Nieuwe vraag toevoegen
                var newQuestion = new Question
                {
                    Text = NewQuestionText,
                    Category = NewQuestionCategory,
                    Level = NewQuestionLevel
                };

                try
                {
                    await _databaseService.AddQuestionAsync(newQuestion);
                    Questions.Add(newQuestion);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fout bij toevoegen van nieuwe vraag: {ex.Message}");
                }
            }
            else
            {
                // Bestaande vraag bijwerken
                SelectedQuestion.Text = NewQuestionText;
                SelectedQuestion.Category = NewQuestionCategory;
                SelectedQuestion.Level = NewQuestionLevel;

                try
                {
                    await _databaseService.UpdateQuestionAsync(SelectedQuestion);

                    var index = Questions.IndexOf(SelectedQuestion);
                    if (index >= 0)
                    {
                        Questions[index] = SelectedQuestion;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fout bij bewerken van vraag: {ex.Message}");
                }
            }

            // Reset formulier
            SelectedQuestion = null;
            NewQuestionText = string.Empty;
            NewQuestionCategory = string.Empty;
            NewQuestionLevel = 1;
            IsAddQuestionFormVisible = false;
        }

    }
}