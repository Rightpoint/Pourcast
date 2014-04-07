namespace RightpointLabs.Pourcast.Infrastructure.Data
{
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.IdGenerators;

    using RightpointLabs.Pourcast.Domain.Models;

    public class MongoClassMapper
    {
        private bool _areClassesMapped = false;

        public void EnsureMappings()
        {
            if (!_areClassesMapped)
            {
                _areClassesMapped = true;

                MapEntity();
                MapKeg();
                MapBeer();
                MapBrewery();
            }
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
                    cm.MapCreator(b => new Brewery(b.Id, b.Name));
                });
        }

        private static void MapBeer()
        {
            BsonClassMap.RegisterClassMap<Beer>(
                cm =>
                {
                    cm.AutoMap();
                    cm.MapCreator(b => new Beer(b.Id, b.Name));
                });
        }

        private static void MapKeg()
        {
            BsonClassMap.RegisterClassMap<Keg>(
                cm =>
                {
                    cm.AutoMap();
                    cm.MapCreator(k => new Keg(k.Id, k.Capacity));
                });
        }
    }
}
