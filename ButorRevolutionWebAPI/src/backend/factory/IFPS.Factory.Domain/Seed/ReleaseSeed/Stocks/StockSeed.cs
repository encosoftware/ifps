using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class StockSeed : IEntitySeed<Stock>
    {
        public Stock[] Entities => new[]
        {
            new Stock(1,5,5) { Id = 1 },
            new Stock(2,5,5) { Id = 2 },
            new Stock(2,5,5) { Id = 3 },
            new Stock(2,5,5) { Id = 4, ValidTo = new DateTime(2019,10,1) },
            new Stock(2,5,5) { Id = 5, ValidTo = new DateTime(2019,8,1) },
            new Stock(2,5,5) { Id = 6, ValidFrom = new DateTime(2019,8,1), ValidTo = new DateTime(2019,11,1)},
            new Stock(2,5,5) { Id = 7 },
            new Stock(3,10,5) { Id = 8 },
            new Stock(3,10,5) { Id = 9 }
        };

        //public Stock[] Entities => new Stock[] { };
    }
}
