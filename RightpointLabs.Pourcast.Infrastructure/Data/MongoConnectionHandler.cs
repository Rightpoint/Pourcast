namespace RightpointLabs.Pourcast.Infrastructure.Data
{
    using MongoDB.Driver;

    using RightpointLabs.Pourcast.Domain.Models;

    public class MongoConnectionHandler : IMongoConnectionHandler
    {
        private MongoServer _server;
        private MongoDatabase _database;

         public MongoConnectionHandler(string connectionString, string database)
         {
             _server = new MongoClient(connectionString).GetServer();
             _database = _server.GetDatabase(database);
             
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