namespace RightpointLabs.Pourcast.Infrastructure.Data
{
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.IdGenerators;

    using RightpointLabs.Pourcast.Domain.Models;

    public class MongoClassMapper
    {
        public void EnsureMappings()
        {
            MapEntity();
            MapKeg();
            MapBeer();
            MapBrewery();
        }

        private static void MapEntity()
        {
            BsonClassMap.RegisterClassMap<Entity>(
                cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                    cm.IdMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
                });
        }

        private static void MapBrewery()
        {
            BsonClassMap.RegisterClassMap<Brewery>(
                cm =>
                {
                    cm.AutoMap();
                });
        }

        private static void MapBeer()
        {
            BsonClassMap.RegisterClassMap<Beer>(
                cm =>
                {
                    cm.AutoMap();
                });
        }

        private static void MapKeg()
        {
            BsonClassMap.RegisterClassMap<Keg>(
                cm =>
                {
                    cm.AutoMap();
                });
        }
    }
}
