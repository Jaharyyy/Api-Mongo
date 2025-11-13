using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using WebAPI.MODEL;

namespace WebAPI.DATAS
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection("MongoDB");
            var connectionString = mongoSettings.GetValue<string>("ConnectionString");
            var dbName = mongoSettings.GetValue<string>("DatabaseName");

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
    }
}
