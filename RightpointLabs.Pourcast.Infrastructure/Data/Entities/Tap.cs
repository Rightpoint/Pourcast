namespace RightpointLabs.Pourcast.Infrastructure.Data.Entities
{
    using MongoDB.Bson.Serialization.Attributes;

    public class Tap
    {
        [BsonElement("TapId")]
        public string Id { get; set; }

        [BsonElement("Name")]
        public TapName Name { get; set; }
    }
}