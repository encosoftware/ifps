using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class AccessoryMaterialFurnitureUnitSeed : IEntitySeed<AccessoryMaterialFurnitureUnit>
    {
        public AccessoryMaterialFurnitureUnit[] Entities => new[]
        {
            new AccessoryMaterialFurnitureUnit(new Guid("5799dc7e-f9cb-47a3-8d1c-3cfbc79af794"), new Guid("34132dce-ac95-47d1-ab24-bef35ae8ea4f"), "Handle", 5) { Id = 1 },
            new AccessoryMaterialFurnitureUnit(new Guid("5799dc7e-f9cb-47a3-8d1c-3cfbc79af794"), new Guid("34132dce-ac95-47d1-ab24-bef35ae8ea4f"), "Caster", 10) { Id = 2 },
            new AccessoryMaterialFurnitureUnit(new Guid("42dfb2f8-c914-48c1-a0b8-cf40806b24db"), new Guid("27ca32c4-38d1-4919-9790-3d8a4c9c4e3d"), "Screw", 42) { Id = 3 },

            new AccessoryMaterialFurnitureUnit(new Guid("51b66111-d87d-457e-ac05-f451e942165f"), new Guid("b2e5b9b1-2b4c-40c5-a178-b6368cf9f364"), "", 30) { Id = 4 },
            new AccessoryMaterialFurnitureUnit(new Guid("51b66111-d87d-457e-ac05-f451e942165f"), new Guid("8c344649-0562-498c-95b2-2a3a784bb770"), "", 5) { Id = 5 },
            new AccessoryMaterialFurnitureUnit(new Guid("8c757afa-2bfa-43a6-94c1-918c19675e64"), new Guid("b2e5b9b1-2b4c-40c5-a178-b6368cf9f364"), "", 30) { Id = 6 },
            new AccessoryMaterialFurnitureUnit(new Guid("8c757afa-2bfa-43a6-94c1-918c19675e64"), new Guid("8c344649-0562-498c-95b2-2a3a784bb770"), "", 10) { Id = 7 },
        };

        //public AccessoryMaterialFurnitureUnit[] Entities => new AccessoryMaterialFurnitureUnit[] { };
    }
}
