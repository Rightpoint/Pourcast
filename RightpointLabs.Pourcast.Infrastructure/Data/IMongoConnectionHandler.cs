namespace RightpointLabs.Pourcast.Infrastructure.Data
{
    using MongoDB.Driver;

    public interface IMongoConnectionHandler
    {
        MongoDatabase Database { get; }
    }
}