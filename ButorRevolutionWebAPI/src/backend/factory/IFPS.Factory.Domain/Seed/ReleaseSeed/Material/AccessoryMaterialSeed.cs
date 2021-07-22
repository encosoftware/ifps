using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class AccessoryMaterialSeed : IEntitySeed<AccessoryMaterial>
    {
        public AccessoryMaterial[] Entities => new[]
        {
            new AccessoryMaterial(true, false, "LX3243HG (seed)") { Id = new Guid("34132dce-ac95-47d1-ab24-bef35ae8ea4f"), CurrentPriceId = 1, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), CategoryId = 1, Description = "kilincs" },
            new AccessoryMaterial(true, false, "LX100200 (seed)") { Id = new Guid("27ca32c4-38d1-4919-9790-3d8a4c9c4e3d"), CurrentPriceId = 2, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), CategoryId = 1, Description = "sarokvas" },

            new AccessoryMaterial(true, false, "LX-001 (seed)") { Id = new Guid("b2e5b9b1-2b4c-40c5-a178-b6368cf9f364"), CurrentPriceId = 2, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), CategoryId = 1, Description = "fatipli" },
            new AccessoryMaterial(true, false, "LX-010 (seed)") { Id = new Guid("8c344649-0562-498c-95b2-2a3a784bb770"), CurrentPriceId = 2, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), CategoryId = 1, Description = "vasalat" }
        };

        //public AccessoryMaterial[] Entities => new AccessoryMaterial[] { };
    }
}
