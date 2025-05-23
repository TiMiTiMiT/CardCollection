using CardCollection.Classes.JsonConverterClasses;
using CardCollection.Classes.Models.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CardCollection.Classes.Models.Magic
{
    internal class MTGCard : ICard
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } // Cardname

        public int Quantity { get; set; } // The amount in the collection

        public int InUse { get; set; } // amount of card used in decks (always less then "Quantity")

        [JsonPropertyName("layout")]
        public string Layout { get; set; } // Card Frame Layout

        [JsonPropertyName("mana_cost")]
        public string ManaCost { get; set; } // Cost of the card eg. 5{R}{B}

        [JsonPropertyName("cmc")]
        [JsonConverter(typeof(StringDoubleToIntConverter))]
        public int CMC { get; set; } // Cost as integer

        [JsonPropertyName("type_line")]
        public string TypeLine {  get; set; } // e.g. Legendary Creature - Human Monk

        [JsonPropertyName("oracle_text")]
        public string OracleText { get; set; } // Text

        [JsonPropertyName("colors")]
        public string[] Colors { get; set; }

        [JsonPropertyName("color_identity")]
        public string[] ColorIdentity { get; set; }

        [JsonPropertyName("keywords")]
        public string[] Keywords { get; set; }

        [JsonPropertyName("reserved")]
        public bool Reserved {  get; set; }

        [JsonPropertyName("game_changer")]
        public bool GameChanger { get; set; }

        [JsonPropertyName("rarity")]
        public string Rarity { get; set; }

        public MTGCard(string name,
            int quantity,
            string layout,
            string manaCost,
            int cmc,
            string typeLine,
            string oracleText,
            string[] colors,
            string[] colorIdentity,
            string[] keywords,
            bool reserved,
            bool gameChanger,
            string rarity)
        {
            Name = name;
            Quantity = quantity;
            InUse = 0;
            Layout = layout;
            ManaCost = manaCost;
            CMC = cmc;
            TypeLine = typeLine;
            OracleText = oracleText;
            Colors = colors;
            ColorIdentity = colorIdentity;
            Keywords = keywords;
            Reserved = reserved;
            GameChanger = gameChanger;
            Rarity = rarity;
        }
    }
}
