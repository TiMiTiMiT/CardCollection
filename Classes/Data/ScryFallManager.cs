using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardCollection.Classes.Data
{
    internal class ScryFallManager
    {
        /* Use a MTG api to get card infomrations
         *https://scryfall.com/docs/api/
         */
        private HttpClient Client;
        string ApiString = "https://api.scryfall.com";

        public ScryFallManager()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.UserAgent.ParseAdd("Card_Collection/1.0");
            Client.DefaultRequestHeaders.Accept.ParseAdd("*/*");
        }

        public async Task<string?> GetCard(string CardName, string param = "exact")
        {
            try
            {
                HttpResponseMessage response = await Client.GetAsync($"{this.ApiString}/cards/named?{param}={CardName}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }
    }
}
