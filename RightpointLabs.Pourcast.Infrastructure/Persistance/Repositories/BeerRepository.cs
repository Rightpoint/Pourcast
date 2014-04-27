namespace RightpointLabs.Pourcast.Infrastructure.Persistance.Repositories
{
    using System.Linq;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistance.Collections;

    public class BeerRepository : EntityRepository<Beer>, IBeerRepository
    {
        public BeerRepository(BeerCollectionDefinition beerCollectionDefinition)
            : base(beerCollectionDefinition)
        {
        }

        public System.Collections.Generic.IEnumerable<Beer> GetAllByName(string name)
        {
            return Queryable.Where(b => b.Name.Contains(name));
        }

        public System.Collections.Generic.IEnumerable<Beer> GetByBreweryId(string breweryId)
        {
            return Queryable.Where(b => b.BreweryId == breweryId);
        }
    }
}