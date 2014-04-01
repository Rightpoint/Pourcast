namespace RightpointLabs.Pourcast.Infrastructure.Data.Entities
{
    using System;

    using MongoDB.Bson.Serialization.Attributes;

    public class Pour
    {
        public DateTime PourDateTime { get; set; }

        [BsonElement("Oz")]
        public double Volume { get; set; } 
    }
}