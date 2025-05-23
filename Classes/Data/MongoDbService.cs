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
    internal class MongoDbService<TCard, TDeck> where TCard : ICard where TDeck : IDeck
    {
        private IMongoCollection<TCard> _cardCollection;
        private IMongoCollection<TDeck> _deckCollection;

        public MongoDbService(string databaseName, string cardCollectionName, string deckCollectionName)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase(databaseName);

            _cardCollection = database.GetCollection<TCard>(cardCollectionName);
            _deckCollection = database.GetCollection<TDeck>(deckCollectionName);
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

        public async Task ExportCardCollection(string GameName)
        {
            string path = $"C:\\Users\\Tim\\source\\repos\\CardCollection\\Savefiles\\Collection\\Collection_{GameName}_Export.txt";

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
            try
            {
                StreamReader StreamReader = new StreamReader("C:\\Users\\Tim\\source\\repos\\CardCollection\\Savefiles\\Collection\\Collection_MTG_Export.txt");
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
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        // ----- Deck methods -----
    }
}
