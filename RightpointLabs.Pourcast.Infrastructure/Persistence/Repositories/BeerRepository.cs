using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using System.Linq;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BeerRepository : TableRepository<Beer>, IBeerRepository
    {
        private const string BreweryIdField = "BreweryId";

        public BeerRepository(CloudTableClient client) : base(client)
        {
        }

        protected override DynamicTableEntity ToTableEntity(Beer entity)
        {
            var e = base.ToTableEntity(entity);
            e[BreweryIdField] = new EntityProperty(entity.BreweryId);
            return e;
        }

        public IEnumerable<Beer> GetAllByName(string name)
        {
            return this.GetAll().Where(i => i.Name == name).ToList();
        }

        public IEnumerable<Beer> GetByBreweryId(string breweryId)
        {
            return _table.ExecuteQuery(new TableQuery<DynamicTableEntity>().Where(FilterConditionByBreweryId(breweryId))).Select(FromTableEntity).ToList();
        }

        protected string FilterConditionByBreweryId(string breweryId)
        {
            return TableQuery.GenerateFilterCondition(BreweryIdField, QueryComparisons.Equal, breweryId);
        }
    }
}