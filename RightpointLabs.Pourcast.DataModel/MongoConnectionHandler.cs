using MongoDB.Driver;
using RightpointLabs.Pourcast.DataModel.Entities;

namespace RightpointLabs.Pourcast.DataModel
{
    public class MongoConnectionHandler : IMongoConnectionHandler
    {
        private MongoServer _server;
        private MongoDatabase _database;
        private MongoCollection _collection;

         public MongoConnectionHandler(string connectionString, string database)
         {
             _server = new MongoClient(connectionString).GetServer();
             _database = _server.GetDatabase(database);
         }

         public MongoCollection Collection { get { return _collection; }}

        public void SetCollection<T>()
        {
            _collection = _database.GetCollection<T>(typeof(T).Name.ToLower() + "s");
        }
    }
}