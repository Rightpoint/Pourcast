namespace RightpointLabs.Pourcast.Infrastructure.Data.Entities
{
    using System.Collections.Generic;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Keg : IMongoEntity
    {
        
        [BsonId]
        public ObjectId Id { get; set; }
        public Beer Beer { get; set; }
        public Status Status { get; set; }
        public Tap Tap { get; set; }
        public IEnumerable<Pour> Pours { get; set; }

    }
}