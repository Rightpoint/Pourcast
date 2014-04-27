namespace RightpointLabs.Pourcast.Infrastructure.Persistance
{
    using MongoDB.Driver;

    public class MongoConnectionHandler : IMongoConnectionHandler
    {
        private readonly MongoDatabase _database;

         public MongoConnectionHandler(string connectionString, string database)
         {
             MongoServer server = new MongoClient(connectionString).GetServer();
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