using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class StockedMaterialTestSeed : IEntitySeed<StockedMaterial>
    {
        public StockedMaterial[] Entities => new[]
        {
            new StockedMaterial(Guid.Parse("c649fc22-247e-4e88-817d-e398c349257b"), 100, 35, 50 ) { Id = 10000 },
            new StockedMaterial(Guid.Parse("9f1f5d74-e58e-4ea4-9f61-1ef37cf91d4b"), 70, 20, 30 ) { Id = 10001 },
            new StockedMaterial(Guid.Parse("b71e3923-3441-46c7-a2ba-13da290ecd6d"), 50, 10, 20 ) { Id = 10002 },
            new StockedMaterial(Guid.Parse("81ae6bc6-a7b2-45d5-bb2c-a1b174f2e1cb"), 1000, 200, 500 ) { Id = 10003 },
            new StockedMaterial(Guid.Parse("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), 110, 60, 45 ) { Id = 10004 }
        };
    }
}