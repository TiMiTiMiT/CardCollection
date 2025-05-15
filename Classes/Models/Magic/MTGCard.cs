using CardCollection.Classes.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardCollection.Classes.Models.Magic
{
    internal class MTGCard : ICard
    {
        public string Name { get; set; } // Cardname
        public int Quantity { get; set; } // The amount in the collection
        public int InUse { get; set; } // amount of card used in decks (always less then "Quantity")
        public string Object {  get; set; } // Card or Token
        public string Layout { get; set; } // Card Frame Layout
        public string ManaCost { get; set; } // Cost of the card eg. 5{R}{B}
        public int CMC { get; set; } // Cost as integer
        public string TypeLine {  get; set; } // e.g. Legendary Creature - Human Monk
        public string OracleText { get; set; } // Text
        public string[] Colors { get; set; }
        public string[] ColorIdentity { get; set; }
        public string[] Keywords { get; set; }
        public bool Reserved {  get; set; }
        public bool GameChanger { get; set; }
        public string Set {  get; set; }
        public string SetName { get; set; }
        public string Rarity { get; set; }

        public MTGCard(string name,
            int quantity,
            string @object,
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
            string set,
            string setName,
            string rarity)
        {
            Name = name;
            Quantity = quantity;
            InUse = 0;
            Object = @object;
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
            Set = set;
            SetName = setName;
            Rarity = rarity;
        }
    }
}
