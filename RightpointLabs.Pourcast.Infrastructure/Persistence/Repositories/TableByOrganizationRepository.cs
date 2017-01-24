using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using RightpointLabs.Pourcast.Domain.Models;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    public class TableByOrganizationRepository<T> : TableRepository<T> where T : Entity, IByOrganizationId
    {
        public TableByOrganizationRepository(CloudTableClient client) : base(client)
        {
        }

        public IEnumerable<T> GetAll(string organizationId)
        {
            return _table.ExecuteQuery(new TableQuery<DynamicTableEntity>().Where(FilterConditionAll(organizationId))).Select(FromTableEntity);
        }

        public string FilterConditionAll(string organizationId)
        {
            return
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, organizationId);

        }

        public T GetById(string organizationId, string id)
        {
            return _table.ExecuteQuery(new TableQuery<DynamicTableEntity>().Where(FilterConditionById(organizationId, id))).Select(FromTableEntity).SingleOrDefault();
        }

        public string FilterConditionById(string organizationId, string id)
        {
            return
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, organizationId),
                    TableOperators.And,
                    FilterConditionById(id));

        }

        public IEnumerable<T> GetById(string organizationId, string[] id)
        {
            return _table.ExecuteQuery(new TableQuery<DynamicTableEntity>().Where(FilterConditionById(organizationId, id))).Select(FromTableEntity);
        }

        public string FilterConditionById(string organizationId, string[] id)
        {
            return
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, organizationId),
                    TableOperators.And,
                    id.Aggregate("", (q, i) =>
                    {
                        var part = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, i);
                        if (string.IsNullOrEmpty(q))
                            return part;
                        return TableQuery.CombineFilters(q, TableOperators.Or, part);
                    }));
        }

        protected override string GetPartitionKey(T entity)
        {
            return entity.OrganizationId;
        }
    }
}