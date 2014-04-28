namespace RightpointLabs.Pourcast.Infrastructure.Persistence
{
    using MongoDB.Driver;

    public interface IMongoConnectionHandler
    {
        MongoDatabase Database { get; }
    }
}