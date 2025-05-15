using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardCollection.Classes.Models.Interfaces
{
    internal interface ICard
    {
        public string Name { get; } // Cardname
        public int Quantity { get; } // The amount in the collection
        public int InUse { get; } // amount of card used in decks (always less then "Quantity")
    }
}
