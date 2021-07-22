using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class AccessoryMaterialSeed : IEntitySeed<AccessoryMaterial>
    {
        public AccessoryMaterial[] Entities => new[]
        {
            new AccessoryMaterial() { Id = new Guid("695bf260-ae0b-47f1-b4c5-3adba851b694"), Code = "VCS100200HL (seed)", CurrentPriceId = 1, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), CategoryId = 1, Description = "20 cm fitting" },
            new AccessoryMaterial() { Id = new Guid("9a0124ac-6b7d-4eb7-89fa-a4a874659dad"), Code = "VSU560TZ (seed)", CurrentPriceId = 2, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), CategoryId = 1, Description = "5mm dowel" }
        };
        //public AccessoryMaterial[] Entities => new AccessoryMaterial[] { };
    }
}
