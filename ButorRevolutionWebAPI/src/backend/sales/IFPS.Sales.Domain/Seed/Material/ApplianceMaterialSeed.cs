using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Domain.Seed
{
    public class ApplianceMaterialSeed : IEntitySeed<ApplianceMaterial>
    {
        public ApplianceMaterial[] Entities => new[]
        {
            new ApplianceMaterial("WT100100 (seed)") { Id = new Guid("d7b28f76-2766-425f-a9b2-b7646ae8c77f"), BrandId = 3, CategoryId = 20, Description = "Alulfagyasztós Total NoFrost hűtőszekrény, A+++ energiaosztály", ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), CurrentPriceId = 1 },
            new ApplianceMaterial("WT540GH (seed)") { Id = new Guid("f3ac5748-9bb5-47f6-8b64-8fa891024fe5"), BrandId = 3, CategoryId = 20, Description = "Keskeny elöltöltős mosógép, A+++ energiaosztály", ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), CurrentPriceId = 1 },

            new ApplianceMaterial("WT1020BN (seed)")
            {
                Id = new Guid("2903aac4-3564-4117-8a21-c7358814c306"),
                Description = "13 teríték, A+, 5 pr., 3 hőm., nyomógombos, 11 l., 49 dB(A), AirDry Technológia",
                BrandId = 3,
                CategoryId = 31,
                HanaCode = "7332543542765",
                ImageId = Guid.Parse("525955cb-f710-401b-bb30-3318e1cea414"),
                CurrentPriceId = 3
            }
        };
        //public ApplianceMaterial[] Entities => new ApplianceMaterial[] { };
    }
}
