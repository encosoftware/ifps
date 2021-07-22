using System;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class CustomerFurnitureUnitSeed : IEntitySeed<CustomerFurnitureUnit>
    {
        public CustomerFurnitureUnit[] Entities => new[]
        {
            new CustomerFurnitureUnit(1, 1) { Id = 1 },
            new CustomerFurnitureUnit(1, 3) { Id = 2 },
            new CustomerFurnitureUnit(1, 1) { Id = 3 },
            new CustomerFurnitureUnit(2, 2) { Id = 4 }
        };
        //public CustomerFurnitureUnit[] Entities => new CustomerFurnitureUnit[] { };
    }
}