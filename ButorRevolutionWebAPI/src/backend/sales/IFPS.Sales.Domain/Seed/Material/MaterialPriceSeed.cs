using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class MaterialPriceSeed : IEntitySeed<MaterialPrice>
    {
        public MaterialPrice[] Entities => new[]{
            new MaterialPrice()
            {
                Id = 1,
                ValidFrom = Clock.Now,
                ValidTo = null,
            },

            new MaterialPrice()
            {
                Id = 2,
                ValidFrom = Clock.Now,
                ValidTo = null,
            },

            new MaterialPrice()
            {
                Id = 3,
                ValidFrom = Clock.Now,
                ValidTo = null,
            },
        };
        //public MaterialPrice[] Entities => new MaterialPrice[] { };
    }
}
