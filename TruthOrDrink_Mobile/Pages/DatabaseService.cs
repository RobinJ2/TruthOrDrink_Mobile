using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace TruthOrDrink_Mobile.Pages
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "TruthOrDrink.db");
            _database = new SQLiteAsyncConnection(dbPath);
            Task.Run(async () => await InitializeDatabase());
        }

        private async Task InitializeDatabase()
        {
            // Tabellen aanmaken
            await _database.CreateTableAsync<Question>();
            await _database.CreateTableAsync<Photo>();

            // Controleer of er al vragen in de database staan
            var existingQuestions = await _database.Table<Question>().ToListAsync();
            Console.WriteLine($"Aantal bestaande vragen: {existingQuestions.Count}");
            if (!existingQuestions.Any())
            {
                // Voeg standaardvragen toe
                var defaultQuestions = new List<Question>
                {
                    new Question { Text = "Wat is je favoriete drankje?", Category = "Algemeen", Level = 1 },
                    new Question { Text = "Wat is je meest gênante moment in een café?", Category = "Persoonlijk", Level = 3 },
                    new Question { Text = "Wat is het gekste dat je ooit hebt gedaan toen je dronken was?", Category = "Humor", Level = 5 }
                };

                await _database.InsertAllAsync(defaultQuestions);
                Console.WriteLine("Standaardvragen toegevoegd.");
            }
        }

        // Vraag-methoden
        public Task<int> AddQuestionAsync(Question question) => _database.InsertAsync(question);
        public Task<List<Question>> GetQuestionsAsync() => _database.Table<Question>().ToListAsync();
        public Task<int> UpdateQuestionAsync(Question question) => _database.UpdateAsync(question);
        public Task<int> DeleteQuestionAsync(Question question) => _database.DeleteAsync(question);

        // Foto-methoden
        public Task<int> AddPhotoAsync(Photo photo) => _database.InsertAsync(photo);
        public Task<List<Photo>> GetPhotosAsync() => _database.Table<Photo>().ToListAsync();
        public Task<int> DeletePhotoAsync(Photo photo) => _database.DeleteAsync(photo);
    }

    // Vraag Model
    public class   Question
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public int Level { get; set; } 
    }

    // Foto Model
    public class Photo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
    }
}