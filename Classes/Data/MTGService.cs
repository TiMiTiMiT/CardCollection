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

        public async Task AddMTGCard(string Card, int Amount) 
        {
            string? response = await this.ScryFallManager.GetCard(Card);

            if (response != null)
            {
                MTGCard card = JsonSerializer.Deserialize<MTGCard>(response);
                card.Quantity = Amount;

                await _collectionDb.AddCard(card);
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
        
    }
}
