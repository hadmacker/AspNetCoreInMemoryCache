using AspNetCoreInMemoryCache.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreInMemoryCache.Web.Services
{
    public interface ICustomerRepository
    {
        Task DeleteEntityAsync(CustomerEntity deleteEntity);
        Task<CustomerEntity> InsertOrMergeEntityAsync(CustomerEntity entity);
        Task<CustomerEntity> RetrieveEntityUsingPointQueryAsync(string rowKey);
        Task<List<CustomerEntity>> RetrieveAllEntities();
    }
}