using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Domain.Repositories;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    public class TableRepository<T>: IRepository where T : Entity
    {
        protected readonly CloudTable _table;

        protected virtual string TableName => typeof(T).Name;

        public TableRepository(CloudTableClient client)
        {
            _table = client.GetTableReference(TableName);
        }

        public void Insert(T item)
        {
            Upsert(item);
        }

        public void Update(T item)
        {
            Upsert(item);
        }

        public void Upsert(T item)
        {
            if (string.IsNullOrEmpty(item.Id))
                throw new ArgumentException("Id must be set");

            _table.Execute(TableOperation.InsertOrReplace(ToTableEntity(item)));
        }

        public T GetById(string id)
        {
            return _table.ExecuteQuery(new TableQuery<DynamicTableEntity>().Where(FilterConditionById(id))).Select(FromTableEntity).SingleOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return _table.ExecuteQuery(new TableQuery<DynamicTableEntity>()).Select(FromTableEntity);
        }

        protected string FilterConditionById(string id)
        {
            return TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id);
        }
        
        protected virtual string GetPartitionKey(T entity)
        {
            return "";
        }

        protected virtual string GetRowKey(T entity)
        {
            return entity.Id;
        }

        protected virtual DynamicTableEntity ToTableEntity(T entity)
        {
            if (null == entity)
                return null;

            var tableEntity = new DynamicTableEntity(GetPartitionKey(entity), GetRowKey(entity)) { ETag = "*" };
            tableEntity.Properties.Add("Data", new EntityProperty(BuildJObject(entity).ToString(Formatting.None)));
            return tableEntity;
        }

        protected virtual JObject BuildJObject(T entity)
        {
            return JObject.FromObject(entity);
        }

        protected virtual T FromTableEntity(DynamicTableEntity tableEntity)
        {
            if (null == tableEntity)
                return null;

            return JObject.Parse(tableEntity.Properties["Data"].StringValue).ToObject<T>();
        }

        public void Init()
        {
            _table.CreateIfNotExists();
        }

        public string NextIdentity()
        {
            return Guid.NewGuid().ToString();
        }
    }
}