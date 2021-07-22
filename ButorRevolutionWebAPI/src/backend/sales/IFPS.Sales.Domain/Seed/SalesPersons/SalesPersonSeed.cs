using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class SalesPersonSeed : IEntitySeed<SalesPerson>
    {
        public SalesPerson[] Entities => new[]
        {
            new SalesPerson(1, Clock.Now) { Id = 1 },
            new SalesPerson(5, Clock.Now) { Id = 2 },
            new SalesPerson(8, Clock.Now) { Id = 3 },
            new SalesPerson(9, Clock.Now) { Id = 4 },
            new SalesPerson(10, Clock.Now) { Id = 5 },
            new SalesPerson(11, Clock.Now) { Id = 6 },
            new SalesPerson(12, Clock.Now) { Id = 7 },
            new SalesPerson(13, Clock.Now) { Id = 8 },
            new SalesPerson(14, Clock.Now) { Id = 9 },
            new SalesPerson(15, Clock.Now) { Id = 10 },
        };
        //public SalesPerson[] Entities => new SalesPerson[] { };
    }
}
