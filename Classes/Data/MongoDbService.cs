using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardCollection.Classes.Models.Interfaces;
using CardCollection.Classes.Models.Magic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CardCollection.Classes.Data
{
    internal class MongoDbService<TCard, TDeck> 
        where TCard : ICard 
        where TDeck : IDeck
    {
        private MongoClient Client;
        private IMongoDatabase Database;
        private IMongoCollection<TCard> _cardCollection;
        private string CardCollectionName;
        private IMongoCollection<TDeck> _deckCollection;
        private string DeckCollectionName;

        public MongoDbService(string databaseName, string cardCollectionName, string deckCollectionName)
        {
            Client = new MongoClient(GlobalVariables.Settings["mongoDB_conection"]);
            Database = Client.GetDatabase(GlobalVariables.Settings["mongoDB_database_name"]);

            CardCollectionName = cardCollectionName;
            _cardCollection = Database.GetCollection<TCard>(cardCollectionName);
            DeckCollectionName = deckCollectionName;
            _deckCollection = Database.GetCollection<TDeck>(deckCollectionName);
        }

        // ----- Card methods -----
        public async Task AddCard(TCard Card) =>
            await _cardCollection.InsertOneAsync(Card);

        public async Task<TCard> FindCardByName(string CardName)
        {
            var Filter = Builders<TCard>.Filter.Eq(Card => Card.Name, CardName);
            TCard? card = await _cardCollection.Find(Filter).FirstOrDefaultAsync();

            return card;
        }

        public async Task RemoveCardByName(string CardName)
        {
            var Filter = Builders<TCard>.Filter.Eq(Card => Card.Name, CardName);
            await _cardCollection.DeleteOneAsync(Filter);
        }

        public async Task IncreaseQuantity(string CardName, int NewAmount) 
        {
            var Filter = Builders<TCard>.Filter.Eq(Card => Card.Name, CardName);
            UpdateDefinition<TCard> UpdateBuilder = Builders<TCard>.Update.Inc(Card => Card.Quantity, NewAmount);

            await _cardCollection.UpdateOneAsync(Filter, UpdateBuilder);
        }

        public async Task DecreaseQuantity(string CardName, int NewAmount) 
        {
            var Filter = Builders<TCard>.Filter.Eq(Card => Card.Name, CardName);
            UpdateDefinition<TCard> UpdateBuilder = Builders<TCard>.Update.Inc(Card => Card.Quantity, NewAmount * (-1));

            await _cardCollection.UpdateOneAsync(Filter, UpdateBuilder);
        }

        public async Task IncreaseInUse(string CardName, int NewAmount) 
        {
            var Filter = Builders<TCard>.Filter.Eq(Card => Card.Name, CardName);
            UpdateDefinition<TCard> UpdateBuilder = Builders<TCard>.Update.Inc(Card => Card.InUse, NewAmount);

            await _cardCollection.UpdateOneAsync(Filter, UpdateBuilder);
        }

        public async Task DecreaseInUse(string CardName, int NewAmount)
        {
            var Filter = Builders<TCard>.Filter.Eq(Card => Card.Name, CardName);
            UpdateDefinition<TCard> UpdateBuilder = Builders<TCard>.Update.Inc(Card => Card.InUse, NewAmount * (-1));

            await _cardCollection.UpdateOneAsync(Filter, UpdateBuilder);
        }

        public async Task DropCardCollection()
        {
            await Database.DropCollectionAsync(CardCollectionName);
        }

        public async Task ExportCardCollection(string GameName)
        {
            string path = $"{GlobalVariables.Settings["save_path"]}\\Collection\\Collection_{GameName}_Export.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.Create(path).Close();

            List<TCard> CollectionExport = await _cardCollection
                                                                .Find(FilterDefinition<TCard>.Empty)
                                                                .Project<TCard>(Builders<TCard>.Projection.Exclude("_id"))
                                                                .ToListAsync();

            using (var writer = new StreamWriter(path, append: false))
            {
                foreach (TCard card in CollectionExport)
                {
                    writer.WriteLine($"{card.Name}:{card.Quantity}");
                }
            }
        }

        public async Task ImportCardCollection(Func<string, int, Task> AddCardGameSpecific)
        {
            // drop collection to reset
            await DropCardCollection();

            // import collection from file
            try
            {
                StreamReader StreamReader = new StreamReader($"{GlobalVariables.Settings["save_path"]}\\Collection\\Collection_MTG_Export.txt");
                String? line;

                while ((line = StreamReader.ReadLine()) != null)
                {
                    string[] lineSplit = line.Split(":");
                    if (lineSplit.Length == 2)
                    {
                        string CardName = lineSplit[0].Trim();
                        string Amount = lineSplit[1].Trim();

                        await AddCardGameSpecific(CardName, Int32.Parse(Amount));
                        // deley of 1 second because the documentation of scrfall ask for 50 - 10 millisecond deley and i want to make sure for now
                        await Task.Delay(TimeSpan.FromSeconds(0.1));
                    }
                }

                StreamReader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        // ----- Deck methods -----
        public async Task AddDeck(TDeck Deck) =>
            await _deckCollection.InsertOneAsync(Deck);

        public async Task<TDeck> FindDeckByName(string DeckName)
        {
            var Filter = Builders<TDeck>.Filter.Eq(Deck => Deck.Name, DeckName);
            TDeck? Deck = await _deckCollection.Find(Filter).FirstOrDefaultAsync();

            return Deck;
        }

        public async Task RemoveDeckByName(string DeckName) 
        {
            var Filter = Builders<TDeck>.Filter.Eq(Deck => Deck.Name, DeckName);
            await _deckCollection.DeleteOneAsync(Filter);
        }

        public async Task DropDeckCollection()
        {
            await Database.DropCollectionAsync(DeckCollectionName);
        }

    }
}
