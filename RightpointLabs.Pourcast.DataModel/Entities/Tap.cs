using MongoDB.Bson.Serialization.Attributes;

namespace RightpointLabs.Pourcast.DataModel.Entities
{
    public class Tap
    {
        [BsonElement("TapId")]
        public int TapId { get; set; }

        [BsonElement("Name")]
        public TapName Name { get; set; } 
    }
}