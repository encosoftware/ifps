using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class FoilMaterialSeed : IEntitySeed<FoilMaterial>
    {
        public FoilMaterial[] Entities => new[]
        {
            new FoilMaterial("EL001 (seed)",10) { Id = new Guid("19399685-faa1-4d34-a336-fad36fd468b6"),  Description = "Premium white foil", Thickness = 2, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), CurrentPriceId = 1  },
            new FoilMaterial("ELB560K (seed)", 10) { Id = new Guid("1390a3f5-a822-48b5-a52e-e82d5e49a4fe"), Description = "Premium brown foil", Thickness = 2, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), CurrentPriceId = 2  },
            new FoilMaterial("ELU101080 (seed)", 10) { Id = new Guid("700b65e5-c5d5-4061-a9fc-d810e68ad006"), Description = "Matt darkgray foil", Thickness = 3, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), CurrentPriceId = 2  },
            new FoilMaterial("EL990DG (seed)", 10) { Id = new Guid("7a240dc3-8ab1-4f98-9eac-b9022f25b71b"), Description = "Premium black foil", Thickness = 4, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), CurrentPriceId = 2  }
        };
        //public FoilMaterial[] Entities => new FoilMaterial[] { };
    }
}
