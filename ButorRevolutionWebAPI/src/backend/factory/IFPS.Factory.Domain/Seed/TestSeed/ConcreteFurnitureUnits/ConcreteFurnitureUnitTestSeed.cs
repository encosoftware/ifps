using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class ConcreteFurnitureUnitTestSeed : IEntitySeed<ConcreteFurnitureUnit>
    {
        public ConcreteFurnitureUnit[] Entities => new[]
        {
            new ConcreteFurnitureUnit(new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887")) { Id = 10000, FurnitureUnitId = new Guid("1a44bdef-80bc-49c7-9b03-6d597bf36e47") },
            new ConcreteFurnitureUnit(new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887")) { Id = 10001, FurnitureUnitId = new Guid("5db3fb96-1619-4b05-afa2-9485c282db76") },

            new ConcreteFurnitureUnit(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"))
            { 
                Id = 10002,
                FurnitureUnitId = new Guid("f4f37e65-379c-4569-a720-83e4f9ec0e90") 
            }
        };
    }
}
