namespace RightpointLabs.Pourcast.Infrastructure.Data
{
    using MongoDB.Driver;

    public interface IMongoConnectionHandler<T>
    {
        MongoCollection<T> MongoCollection { get; }
    }
}