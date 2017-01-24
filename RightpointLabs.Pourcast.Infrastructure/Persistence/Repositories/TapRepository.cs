using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class TapRepository : TableByOrganizationRepository<Tap>, ITapRepository
    {
        public TapRepository(CloudTableClient client) : base(client)
        {
        }

        public Tap GetByName(string name, string organizationId)
        {
            return this.GetAll(organizationId).SingleOrDefault(i => i.Name == name);
        }
    }
}