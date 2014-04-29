namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistence.Collections;

    public class KegRepository : EntityRepository<Keg>,  IKegRepository
    {
        public KegRepository(KegCollectionDefinition kegCollectionDefinition)
            : base(kegCollectionDefinition)
        {
        }

        public IEnumerable<Keg> GetAll(bool isEmpty)
        {
            return isEmpty ? Queryable.Where(k => (k.Capacity - k.AmountOfBeerPoured).Equals(0)) : Queryable.Where(k => (k.Capacity - k.AmountOfBeerPoured) > 0);
        }
    }
}