using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class StockTestSeed : IEntitySeed<Stock>
    {
        public Stock[] Entities => new[]
        {
            new Stock(10000,5,10000) { Id = 10000 },
            new Stock(10001,5,10000) { Id = 10001 },
            new Stock(10001,5,10000) { Id = 10002 },
            new Stock(10001,11,10000) { Id = 10003 },
            new Stock(10001,5,10001) { Id = 10004 },
            new Stock(10001,5,10001) { Id = 10005 },
            new Stock(10001,5,10001) { Id = 10006 },
            new Stock(10001,10,10000) { Id = 10007 },
            new Stock(10002,10,10000) { Id = 10008 }
        };
    }
}