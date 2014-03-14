using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RightpointLabs.Pourcast.DataModel.Entities
{
    public class Brewery
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Website { get; set; }
        public string Logo { get; set; }
    }
}
