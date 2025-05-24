using CardCollection.Classes.Models.Interfaces;
using CardCollection.Classes.Models.Magic;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CardCollection.Classes.Data
{
    public class MTGService
    {
        private readonly MongoDbService<MTGCard, MTGDeck> _collectionDb;
        private ScryFallManager ScryFallManager;

        public MTGService()
        {
            _collectionDb = new MongoDbService<MTGCard, MTGDeck>("card_database", "MTGCardCollection", "MTGDeckCollection");
            ScryFallManager = new ScryFallManager();
        }

        // ----- Card methods -----
        public async Task AddMTGCard(string CardName, int Amount) 
        {
            string? response = await this.ScryFallManager.GetCard(CardName);

            if (response != null)
            {
                MTGCard Card = JsonSerializer.Deserialize<MTGCard>(response);
                // change name because some cards have 2 name s in one and should only have one of those for usability. For more information check the Wiki
                Card.Name = CardName;
                Card.Quantity = Amount;

                bool CardExists = await GlobalVariables.MTGService.DoesCardExistInCollection(Card.Name);
                if (CardExists)
                {
                    await _collectionDb.IncreaseQuantity(Card.Name, Amount);
                }
                else
                {
                    await _collectionDb.AddCard(Card);
                }
            }
        }

        public async Task<bool> DoesCardExistInCollection(string CardName)
        {
            MTGCard Card = await _collectionDb.FindCardByName(CardName);
            if (Card != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task RemoveMTGCardByName(string CardName) =>
            await _collectionDb.RemoveCardByName(CardName);

        public async Task IncreaseQuantityByName(string CardName, int NewAmount) =>
            await _collectionDb.IncreaseQuantity(CardName, NewAmount);

        public async Task DecreaseQuantityByName(string CardName, int NewAmount) =>
            await _collectionDb.DecreaseQuantity(CardName, NewAmount);

        public async Task IncreaseInUseByName(string CardName, int NewAmount) =>
            await _collectionDb.IncreaseInUse(CardName, NewAmount);


        public async Task DecreaseInUseByName(string CardName, int NewAmount) =>
            await _collectionDb.DecreaseInUse(CardName, NewAmount);

        public async Task ExportCardCollection() =>
            await _collectionDb.ExportCardCollection("MTG");


        public async Task ImportCardCollection() =>
            await _collectionDb.ImportCardCollection(AddMTGCard);

        // ----- Deck methods -----
        public async Task AddDeck(MTGDeck Deck)
        {
            // create a combined dictionary from Sideboard and Deck/Maindeck
            // because cards can be in both dicts
            Dictionary<string, int> CombinedDeck = new Dictionary<string, int>(Deck.Deck);

            if (Deck.Sideboard != null)
            {
                foreach (var Card in Deck.Sideboard)
                {
                    if (CombinedDeck.ContainsKey(Card.Key))
                        CombinedDeck[Card.Key] += Card.Value;
                    else
                        CombinedDeck[Card.Key] = Card.Value;
                }
            }

            // check if the needed amount of cards are in the card collection
            foreach (var CardFromDecklist in CombinedDeck)
            {
                MTGCard Card = await _collectionDb.FindCardByName(CardFromDecklist.Key);

                if (Card.Quantity <= (CardFromDecklist.Value + Card.InUse))
                {
                    // stop the function because deck cant be added to collection
                }
            }

            // update InUse for every card in the deck
            foreach (var Card in CombinedDeck)
            {
                await _collectionDb.IncreaseInUse(Card.Key, Card.Value);
            }

            // add deck to deck collection
            await _collectionDb.AddDeck(Deck);
        }
            

        public async Task RemoveDeckByName(string DeckName) 
        {
            // First reduce the InUse of every card in the deck
            MTGDeck Deck = await _collectionDb.FindDeckByName(DeckName);

            // create a combined dictionary from maindeck and sideboard
            Dictionary<string, int> CombinedDeck = new Dictionary<string, int>(Deck.Deck);

            if (Deck.Sideboard != null)
            {
                foreach (var Card in Deck.Sideboard)
                {
                    if (CombinedDeck.ContainsKey(Card.Key))
                        CombinedDeck[Card.Key] += Card.Value;
                    else
                        CombinedDeck[Card.Key] = Card.Value;
                }
            }

            // reduce inUse
            foreach (var Card in CombinedDeck)
            {
                await _collectionDb.DecreaseInUse(Card.Key, Card.Value);
            }

            await _collectionDb.RemoveDeckByName(DeckName);
        }
            
    }
}
