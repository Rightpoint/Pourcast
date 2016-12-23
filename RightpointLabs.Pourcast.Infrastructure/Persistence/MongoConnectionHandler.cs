using System.Security.Authentication;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence
{
    using MongoDB.Driver;

    public class MongoConnectionHandler : IMongoConnectionHandler
    {
        private readonly MongoDatabase _database;

         public MongoConnectionHandler(string connectionString, string database)
         {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoServer server = new MongoClient(settings).GetServer();

            _database = server.GetDatabase(database);
         }

        public MongoDatabase Database
        {
            get
            {
                return _database;
            }
        }
    }
}