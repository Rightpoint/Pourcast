namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using MongoDB.Bson.Serialization;
    using System.Linq;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BeerRepository : EntityRepository<Beer>, IBeerRepository
    {
        static BeerRepository()
        {
            BsonClassMap.RegisterClassMap<Beer>(
                cm =>
                {
                    cm.AutoMap();
                    cm.MapCreator(b => new Beer(b.Id, b.Name));
                });
        }

        public BeerRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }


        public System.Collections.Generic.IEnumerable<Beer> GetAllByName(string name)
        {
            return Queryable.Where(b => b.Name.Contains(name));
        }
    }
}