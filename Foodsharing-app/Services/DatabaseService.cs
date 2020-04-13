using Foodsharing_app.Models;
using MongoDB.Driver;

namespace Foodsharing_app.Services
{
    public class DatabaseService
    {
        public IMongoDatabase Database { get; }

        public DatabaseService(IFoodSharingDatabaseSettings settings)
        {
            var credential = MongoCredential.CreateCredential(settings.MasterDatabaseName, settings.DatabaseUser,
                settings.DatabasePassword);
            var mongoClientSettings = new MongoClientSettings
            {
                Credential = credential,
                Server = new MongoServerAddress(settings.DatabaseServer, settings.DatabasePort)
            };
            var client = new MongoClient(mongoClientSettings);
            Database = client.GetDatabase(settings.DatabaseName);
        }
    }
}