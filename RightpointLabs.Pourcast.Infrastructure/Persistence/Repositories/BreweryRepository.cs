using Microsoft.WindowsAzure.Storage.Table;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BreweryRepository : TableRepository<Brewery>, IBreweryRepository
    {
        public BreweryRepository(CloudTableClient client) : base(client)
        {
        }

        public Brewery GetByName(string name)
        {
            return GetAll().SingleOrDefault(e => e.Name == name);
        }
    }
}