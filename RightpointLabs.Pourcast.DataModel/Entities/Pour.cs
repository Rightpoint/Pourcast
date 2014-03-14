using System;
using MongoDB.Bson.Serialization.Attributes;

namespace RightpointLabs.Pourcast.DataModel.Entities
{
    public class Pour
    {
        public DateTime PourDateTime { get; set; }

        [BsonElement("Oz")]
        public double Volume { get; set; } 
    }
}