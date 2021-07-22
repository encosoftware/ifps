using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class MaterialPriceTestSeed : IEntitySeed<MaterialPrice>
    {
        public MaterialPrice[] Entities => new[]{
            new MaterialPrice()
            {
                Id = 10000,
                ValidFrom = Clock.Now,
                ValidTo = null,
            },

            new MaterialPrice()
            {
                Id = 10001,
                ValidFrom = Clock.Now,
                ValidTo = null,
            }
        };
    }
}
