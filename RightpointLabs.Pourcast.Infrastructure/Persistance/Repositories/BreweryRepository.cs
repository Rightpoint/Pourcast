namespace RightpointLabs.Pourcast.Infrastructure.Persistance.Repositories
{
    using System;
    using System.Linq;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistance.Collections;

    public class BreweryRepository : EntityRepository<Brewery>, IBreweryRepository
    {
        public BreweryRepository(BreweryCollectionDefinition breweryCollectionDefinition)
            : base(breweryCollectionDefinition)
        {
        }


        public Brewery GetByName(string name)
        {
            try
            {
                return Queryable.Single(e => e.Name == name);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}