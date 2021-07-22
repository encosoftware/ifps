using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
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
            }
        };

        //public MaterialPrice[] Entities => new MaterialPrice[] { };
    }
}
