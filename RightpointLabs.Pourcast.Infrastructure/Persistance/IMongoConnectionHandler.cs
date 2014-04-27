namespace RightpointLabs.Pourcast.Infrastructure.Persistance
{
    using MongoDB.Driver;

    public interface IMongoConnectionHandler
    {
        MongoDatabase Database { get; }
    }
}