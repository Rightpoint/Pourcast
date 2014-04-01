namespace RightpointLabs.Pourcast.Infrastructure.Data.Entities
{
    using MongoDB.Bson;

    public interface IMongoEntity
    {
        ObjectId Id { get; set; }
    }
}