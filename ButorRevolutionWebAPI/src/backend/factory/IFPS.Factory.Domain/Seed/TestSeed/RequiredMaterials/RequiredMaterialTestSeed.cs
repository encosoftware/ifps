using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class RequiredMaterialTestSeed : IEntitySeed<RequiredMaterial>
    {
        public RequiredMaterial[] Entities => new[]
        {
            new RequiredMaterial(new Guid("9aa53060-4b7a-4f82-8784-2bcc6313fbd3"), new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), 5) { Id = 10000 },
            new RequiredMaterial(new Guid("9aa53060-4b7a-4f82-8784-2bcc6313fbd3"), new Guid("8b0936ec-0e49-4473-bb1d-2b8bb0564d34"), 2) { Id = 10001 },
            new RequiredMaterial(new Guid("9aa53060-4b7a-4f82-8784-2bcc6313fbd3"), new Guid("3c3fa639-6451-49d7-b7b2-965092ebccf1"), 3) { Id = 10002 }
        };
    }
}
