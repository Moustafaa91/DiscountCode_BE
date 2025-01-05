using MongoDB.Driver;

namespace DiscountCodeService.Data
{
    public class MongoDBContext
    {
        public IMongoDatabase Database { get; }

        public MongoDBContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            Database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return Database.GetCollection<T>(collectionName);
        }
    }
}
