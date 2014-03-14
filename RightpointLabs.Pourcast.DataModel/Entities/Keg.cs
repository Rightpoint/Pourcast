using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RightpointLabs.Pourcast.DataModel.Entities
{
    public class Keg : IMongoEntity
    {
        
        [BsonId]
        public ObjectId Id { get; set; }
        public Beer Beer { get; set; }
        public Status Status { get; set; }
        public Tap Tap { get; set; }
        public IList<Pour> Pours { get; set; }

    }
}