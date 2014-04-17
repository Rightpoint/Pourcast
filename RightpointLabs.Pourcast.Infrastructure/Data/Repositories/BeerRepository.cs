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
            if (!BsonClassMap.IsClassMapRegistered(typeof(Beer)))
            {
                BsonClassMap.RegisterClassMap<Beer>(
                    cm =>
                    {
                        cm.AutoMap();
                        cm.MapCreator(b => new Beer(b.Id, b.Name));
                    });
            }
        }

        public BeerRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

<<<<<<< HEAD
<<<<<<< HEAD
=======

>>>>>>> Adding create beer files
=======
>>>>>>> Can create brewery and can add/create beers for the brewery.  Also, added commands
        public System.Collections.Generic.IEnumerable<Beer> GetAllByName(string name)
        {
            return Queryable.Where(b => b.Name.Contains(name));
        }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> Can create brewery.


        public System.Collections.Generic.IEnumerable<Beer> GetByBreweryId(string breweryId)
        {
            return Queryable.Where(b => b.BreweryId == breweryId);
        }
<<<<<<< HEAD
=======
>>>>>>> Adding create beer files
=======
>>>>>>> Can create brewery.
    }
}