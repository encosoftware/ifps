using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class FurnitureUnitPriceSeed : IEntitySeed<FurnitureUnitPrice>
    {
        public FurnitureUnitPrice[] Entities => new[]
        {
            new FurnitureUnitPrice() { Id = 1 },
            new FurnitureUnitPrice() { Id = 2 }
        };
        
        //public FurnitureUnitPrice[] Entities => new FurnitureUnitPrice[] { };
    }
}
