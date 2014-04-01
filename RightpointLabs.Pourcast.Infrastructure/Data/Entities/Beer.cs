namespace RightpointLabs.Pourcast.Infrastructure.Data.Entities
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Beer : IMongoEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        [BsonElement("Brewery")]
        public Brewery Brewer { get; set; }
        public double ABV { get; set; }
        public int BAScore { get; set; }
        public int RPScore { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Glass { get; set; }
        public string Slug { get; set; }
        
    }
}