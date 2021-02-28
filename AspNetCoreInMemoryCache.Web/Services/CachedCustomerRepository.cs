using AspNetCoreInMemoryCache.Web.Models;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreInMemoryCache.Web.Services
{
    public class CachedCustomerRepository : ICustomerRepository
    {
        // https://docs.microsoft.com/en-us/azure/cosmos-db/tutorial-develop-table-dotnet

        private readonly CloudStorageAccount StorageAccount;
        private string TableName = "Customers";
        private readonly CloudTableClient TableClient;
        private readonly CloudTable Table;
        private readonly ILogger<CustomerRepository> log;
        private readonly IMemoryCache memoryCache;
        private readonly string PartitionKey = "Partition1";
        private readonly string cacheKeyEntities = "entityList_Partition1";

        public CachedCustomerRepository(IConfiguration configuration, ILogger<CustomerRepository> log, IMemoryCache memoryCache)
        {
            StorageAccount = CloudStorageAccount.Parse(configuration["ancimcstorage"]);
            TableClient = StorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            Table = TableClient.GetTableReference(TableName);
            if (Table.CreateIfNotExists())
            {
                // Table Created
            }
            else
            {
                // Table already exists
            }

            this.log = log;
            this.memoryCache = memoryCache;
        }

        public async Task<CustomerEntity> InsertOrMergeEntityAsync(CustomerEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            try
            {
                entity.PartitionKey = PartitionKey;
                entity.RowKey = $"{entity.LastName}_{entity.FirstName}";
                var operation = TableOperation.InsertOrMerge(entity);
                var result = await Table.ExecuteAsync(operation);
                var insertedCustomer = result.Result as CustomerEntity;
                if (result.RequestCharge.HasValue)
                {
                    log.LogInformation("Request charge of InsertOrMerge Operation: ", result.RequestCharge);
                }
                return insertedCustomer;
            }
            catch (Exception e)
            {
                log.LogWarning(e.Message);
                log.LogDebug(e.ToString());
                throw;
            }
        }
        public async Task<CustomerEntity> RetrieveEntityUsingPointQueryAsync(string rowKey)
        {
            try
            {
                var retrieveOperation = TableOperation.Retrieve<CustomerEntity>(PartitionKey, rowKey);
                var result = await Table.ExecuteAsync(retrieveOperation);
                var customer = result.Result as CustomerEntity;
                if (result.RequestCharge.HasValue)
                {
                    log.LogInformation("Request charge of Retrieve Operation: ", result.RequestCharge);
                }
                return customer;
            }
            catch (StorageException e)
            {
                log.LogWarning(e.Message);
                log.LogDebug(e.ToString());
                throw;
            }
        }

        public async Task DeleteEntityAsync(CustomerEntity deleteEntity)
        {
            try
            {
                if (deleteEntity == null)
                {
                    throw new ArgumentNullException("deleteEntity");
                }

                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                TableResult result = await Table.ExecuteAsync(deleteOperation);

                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of Delete Operation: " + result.RequestCharge);
                }

            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public async Task<List<CustomerEntity>> RetrieveAllEntities()
        {
            var cacheEntry = await memoryCache.GetOrCreateAsync(cacheKeyEntities, entry => {
                entry.SlidingExpiration = TimeSpan.FromSeconds(60);
                try
                {
                    var query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey));
                    var result = Table.ExecuteQuery(query);
                    return Task.FromResult(result.ToList());
                }
                catch (StorageException e)
                {
                    log.LogWarning(e.Message);
                    log.LogDebug(e.ToString());
                    throw;
                }
            });

             return cacheEntry;
        }
    }
}
