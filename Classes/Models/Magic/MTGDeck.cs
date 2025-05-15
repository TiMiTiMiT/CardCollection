using CardCollection.Classes.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardCollection.Classes.Models.Magic
{
    internal class MTGDeck : IDeck
    {
        public string Name { get; set; }
        public Dictionary<string, int> Deck {  get; set; }
        public Dictionary<string , int> Sideboard { get; set; }
        public string Format {  get; set; }

        public MTGDeck(string name, Dictionary<string, int> deck, Dictionary<string, int> sideboard, string format)
        {
            Name = name;
            Deck = deck;
            Sideboard = sideboard;
            Format = format;
        }
    }
}
