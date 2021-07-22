using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class FurnitureUnitPriceTestSeed : IEntitySeed<FurnitureUnitPrice>
    {
        public FurnitureUnitPrice[] Entities => new[]
        {
            new FurnitureUnitPrice() { Id = 10000 },
            new FurnitureUnitPrice() { Id = 10001 }
        };
    }
}
