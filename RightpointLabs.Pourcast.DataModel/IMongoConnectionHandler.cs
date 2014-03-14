using MongoDB.Driver;

namespace RightpointLabs.Pourcast.DataModel
{
    public interface IMongoConnectionHandler
    {
        MongoCollection Collection { get; }
        void SetCollection<T>();
    }
}