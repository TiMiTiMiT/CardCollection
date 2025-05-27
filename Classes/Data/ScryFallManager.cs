using CardCollection.Classes.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CardCollection.Classes.Data
{
    public class ScryFallManager
    {
        /* Use a MTG api to get card infomrations
         *https://scryfall.com/docs/api/
         */
        private readonly HttpClient _client;
        private readonly string _apiBaseUrl = "https://api.scryfall.com";

        public ScryFallManager(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Card_Collection/1.0");
            _client.DefaultRequestHeaders.Accept.ParseAdd("*/*");
        }

        public async Task<string?> GetCard(string CardName, string param = "exact")
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_apiBaseUrl}/cards/named?{param}={CardName}");
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new CardNotFoundException("Card was not Found");
                }

                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException();
            }
        }
    }
}
