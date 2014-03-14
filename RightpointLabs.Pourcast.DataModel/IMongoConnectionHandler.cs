using MongoDB.Driver;
using RightpointLabs.Pourcast.DataModel.Entities;

namespace RightpointLabs.Pourcast.DataModel
{
    public interface IMongoConnectionHandler<T> where T : IMongoEntity
    {
        MongoCollection<T> MongoCollection { get; }
    }
}