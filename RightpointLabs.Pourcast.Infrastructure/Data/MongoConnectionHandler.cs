namespace RightpointLabs.Pourcast.Infrastructure.Data
{
    using MongoDB.Driver;

    using RightpointLabs.Pourcast.Infrastructure.Data.Entities;

    public class MongoConnectionHandler<T> : IMongoConnectionHandler<T> where T : IMongoEntity
    {
        protected readonly MongoCollection<T> Collection;
        private MongoServer _server;
        private MongoDatabase _database;

         public MongoConnectionHandler(string connectionString, string database)
         {
             _server = new MongoClient(connectionString).GetServer();
             _database = _server.GetDatabase(database);
             Collection = _database.GetCollection<T>(typeof (T).Name.ToLower() + "s");
         }

        public MongoCollection<T> MongoCollection
        {
            get { return Collection; }
        }
    }
}