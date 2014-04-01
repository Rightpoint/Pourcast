namespace RightpointLabs.Pourcast.Infrastructure.Data
{
    using MongoDB.Driver;

    using RightpointLabs.Pourcast.Infrastructure.Data.Entities;

    public interface IMongoConnectionHandler<T> where T : IMongoEntity
    {
        MongoCollection<T> MongoCollection { get; }
    }
}