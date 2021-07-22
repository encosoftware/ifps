using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class RequiredMaterialSeed : IEntitySeed<RequiredMaterial>
    {
        public RequiredMaterial[] Entities => new[]
        {
            new RequiredMaterial(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"), new Guid("4a7b9b0a-2299-4bb2-95ad-4f1b0f23a47f"), 5) { Id = 1 },
            new RequiredMaterial(new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"), new Guid("4a7b9b0a-2299-4bb2-95ad-4f1b0f23a47f"), 2) { Id = 2 },
            new RequiredMaterial(new Guid("409f08ee-7fad-43d2-a6cb-e0ed274d9cb9"), new Guid("0b02521a-1442-4eae-a41a-b65623502b60"), 3) { Id = 3 },
            new RequiredMaterial(new Guid("0c60cdbc-fce3-4833-8ffa-d46664a68da3"), new Guid("15b3184b-a389-4daf-b562-53d9ea4ad3bf"), 2) { Id = 4 },
            new RequiredMaterial(new Guid("0c60cdbc-fce3-4833-8ffa-d46664a68da3"), new Guid("4b6fb38f-c242-4693-8614-8b684ce8de45"), 1) { Id = 5 },
            new RequiredMaterial(new Guid("0c60cdbc-fce3-4833-8ffa-d46664a68da3"), new Guid("8c344649-0562-498c-95b2-2a3a784bb770"), 8) { Id = 6 },
            new RequiredMaterial(new Guid("0c60cdbc-fce3-4833-8ffa-d46664a68da3"), new Guid("b2e5b9b1-2b4c-40c5-a178-b6368cf9f364"), 20) { Id = 7 }
        };

        //public RequiredMaterial[] Entities => new RequiredMaterial[] { };
    }
}
