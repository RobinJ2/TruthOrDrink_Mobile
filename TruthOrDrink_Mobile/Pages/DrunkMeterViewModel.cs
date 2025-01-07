using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Media;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Timers;


namespace TruthOrDrink_Mobile.Pages
{
    public partial class DrunkMeterViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private readonly QuoteService _quoteService;
        private readonly System.Timers.Timer _quoteTimer;

        [ObservableProperty]
        private ObservableCollection<Photo> photos;

        [ObservableProperty]
        private string stabilityMessage;

        [ObservableProperty]
        private string drunkScoreMessage;

        private bool _isAccelerometerRunning;
        private double _totalMovement = 0;
        private int _movementSamples = 0;

        


        [ObservableProperty]
        private string currentQuote;

        public DrunkMeterViewModel(QuoteService quoteService, DatabaseService databaseService)
        {
            _quoteService = quoteService;
            _databaseService = databaseService;

            // Start de timer
            _quoteTimer = new System.Timers.Timer(10000); // 10000ms = 10 seconden
            _quoteTimer.Elapsed += async (s, e) => await UpdateQuote();
            _quoteTimer.AutoReset = true;
            _quoteTimer.Start();

            // Eerste quote laden
            Task.Run(async () => await UpdateQuote());

            // Foto's uit database laden
            LoadPhotos();
        }

        

        private async Task UpdateQuote()
        {
            CurrentQuote = await _quoteService.GetRandomQuoteAsync();
        }

        // Accelerometer Test
        [RelayCommand]
        private void StartAccelerometerTest()
        {
            if (Accelerometer.IsSupported)
            {
                StabilityMessage = "Test gestart! Houd je telefoon stil...";
                DrunkScoreMessage = ""; // Reset de vorige score
                _totalMovement = 0;
                _movementSamples = 0;

                Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.Start(SensorSpeed.UI);
                _isAccelerometerRunning = true;

                // Stop test na 5 seconden
                Task.Delay(5000).ContinueWith(t => StopAccelerometerTest());
            }
            else
            {
                StabilityMessage = "Accelerometer wordt niet ondersteund op dit apparaat.";
            }
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            double magnitude = Math.Sqrt(data.Acceleration.X * data.Acceleration.X +
                                         data.Acceleration.Y * data.Acceleration.Y +
                                         data.Acceleration.Z * data.Acceleration.Z);

            _totalMovement += Math.Abs(magnitude - 1.0); // Verschil van standaardzwaartekracht
            _movementSamples++;
        }

        private void StopAccelerometerTest()
        {
            if (_isAccelerometerRunning)
            {
                Accelerometer.Stop();
                Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
                _isAccelerometerRunning = false;

                // Bereken gemiddelde beweging
                double averageMovement = _totalMovement / _movementSamples;
                int score = CalculateDrunkScore(averageMovement);

                // Update bericht
                StabilityMessage = "Test voltooid!";
                DrunkScoreMessage = GetDrunkMessage(score);
            }
        }

        private int CalculateDrunkScore(double averageMovement)
        {
            if (averageMovement < 0.02) return 1; // Stabiel
            if (averageMovement < 0.05) return 3; // Lichte beweging
            if (averageMovement < 0.10) return 5; // Enige beweging
            if (averageMovement < 0.20) return 7; // Veel beweging
            return 10; // Zeer veel beweging
        }

        private string GetDrunkMessage(int score)
        {
            return score switch
            {
                <= 3 => $"Score: {score}/10\nJe bent nog niet dronken!",
                <= 7 => $"Score: {score}/10\nJe bent hoogstwaarschijnlijk aangeschoten!",
                _ => $"Score: {score}/10\nJe bent dronken!",
            };
        }

        // Foto maken
        [RelayCommand]
        private async Task TakePhoto()
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync();
                if (result != null)
                {
                    var photoPath = result.FullPath;

                    var newPhoto = new Photo
                    {
                        FilePath = photoPath,
                        Description = "Foto beschrijving"
                    };

                    await _databaseService.AddPhotoAsync(newPhoto);
                    LoadPhotos();

                    await Application.Current.MainPage.DisplayAlert("Foto gemaakt", "Je foto is opgeslagen.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Kan geen foto maken: {ex.Message}", "OK");
            }
        }

        // Vergroot de foto (toon als een pop-up)
        [RelayCommand]
        private async Task ShowPhoto(string photoPath)
        {
            await Application.Current.MainPage.DisplayAlert("Vergrote Foto", $"Foto: {photoPath}", "Sluiten");
            // Hier kan je eventueel een pop-up of modal pagina openen met de vergrote foto.
        }

        private async void LoadPhotos()
        {
            var photoList = await _databaseService.GetPhotosAsync();
            Photos = new ObservableCollection<Photo>(photoList);
        }

        //[RelayCommand]
        //private async Task AddPhoto(string filePath, string description)
        //{
        //    var photo = new Photo { FilePath = filePath, Description = description };
        //    await _databaseService.AddPhotoAsync(photo);
        //    LoadPhotos();
        //}

        [RelayCommand]
        public async Task DeletePhoto(Photo photo)
        {
            await _databaseService.DeletePhotoAsync(photo);
            LoadPhotos();
        }
    }
}