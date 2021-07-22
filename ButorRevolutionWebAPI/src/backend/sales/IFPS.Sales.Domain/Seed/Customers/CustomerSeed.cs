using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class CustomerSeed : IEntitySeed<Customer>
    {
        public Customer[] Entities => new[]
        {
            new Customer(1, Clock.Now) { UserId = 1, Id = 1 },
            new Customer(4, Clock.Now) { Id = 2 },
            new Customer(3, Clock.Now) { Id = 3 },
            new Customer(6, Clock.Now) { Id = 4 },
        };
        //public Customer[] Entities => new Customer[] { };
    }
}
