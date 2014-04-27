namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using System.Linq;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistence.Collections;

    public class RoleRepository : EntityRepository<Role>, IRoleRepository
    {
        public RoleRepository(RoleCollectionDefinition roleCollectionDefinition)
            : base(roleCollectionDefinition)
        {
        }

        public Role GetByName(string name)
        {
            return Queryable.SingleOrDefault(x => x.Name == name);
        }
    }
}
