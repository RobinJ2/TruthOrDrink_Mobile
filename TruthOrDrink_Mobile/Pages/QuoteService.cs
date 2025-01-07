using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace TruthOrDrink_Mobile.Pages
{
    public class QuoteService
    {
        private readonly HttpClient _httpClient;

        public QuoteService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetRandomQuoteAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://zenquotes.io/api/random");
                var quotes = JsonConvert.DeserializeObject<QuoteResponse[]>(response);

                if (quotes != null && quotes.Length > 0)
                {
                    return $"{quotes[0].Q} - {quotes[0].A}";
                }

                return "Geen quote beschikbaar.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij ophalen van quote: {ex.Message}");
                return "Fout bij laden van quote.";
            }

        }

        private class QuoteResponse
        {
            [JsonProperty("q")]
            public string Q { get; set; } // De quote

            [JsonProperty("a")]
            public string A { get; set; } // De auteur
        }
    }
}