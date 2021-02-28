using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreInMemoryCache.Web.Models
{
    public class CustomerEntity : TableEntity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public CustomerEntity()
        {
        }

        public CustomerEntity(string lastName, string firstName)
        {
            LastName = lastName;
            FirstName = firstName;
        }
    }
}
