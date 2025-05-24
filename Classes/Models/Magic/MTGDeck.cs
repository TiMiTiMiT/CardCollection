using CardCollection.Classes.Models.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CardCollection.Classes.Models.Magic
{
    public class MTGDeck : IDeck
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("deck")]
        public Dictionary<string, int> Deck {  get; set; }

        [JsonPropertyName("sideboard")]
        public Dictionary<string , int>? Sideboard { get; set; }

        [JsonPropertyName("format")]
        public string Format {  get; set; }

        [JsonPropertyName("commander")]
        public string? Commander { get; set; }

        public MTGDeck(
            string name, 
            Dictionary<string, int> deck, 
            string format, 
            Dictionary<string, int>? sideboard = null, 
            string? commander = null
           )
        {
            Name = name;
            Deck = deck;
            Sideboard = sideboard;
            Format = format;
            Commander = commander;
        }
    }
}
