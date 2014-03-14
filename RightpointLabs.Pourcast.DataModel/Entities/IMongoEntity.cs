using MongoDB.Bson;

namespace RightpointLabs.Pourcast.DataModel.Entities
{
    public interface IMongoEntity
    {
        ObjectId Id { get; set; }
    }
}