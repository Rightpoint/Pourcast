namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistence.Collections;

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