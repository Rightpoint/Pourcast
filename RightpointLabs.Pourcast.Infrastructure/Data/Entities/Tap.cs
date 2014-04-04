namespace RightpointLabs.Pourcast.Infrastructure.Data.Entities
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Tap : IMongoEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Name")]
        public TapName Name { get; set; }

        public Keg Keg { get; set; }
    }
}