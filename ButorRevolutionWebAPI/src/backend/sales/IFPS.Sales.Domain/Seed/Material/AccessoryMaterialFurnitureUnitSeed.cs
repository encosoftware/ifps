using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class AccessoryMaterialFurnitureUnitSeed : IEntitySeed<AccessoryMaterialFurnitureUnit>
    {
        public AccessoryMaterialFurnitureUnit[] Entities => new[]
        {
            new AccessoryMaterialFurnitureUnit() { Id = 1, AccessoryId = new Guid("695bf260-ae0b-47f1-b4c5-3adba851b694"), FurnitureUnitId = new Guid("ebf87e4c-50b8-402c-b694-ad29183e0bb1"), Name = "Acc-Mat-Name-001", AccessoryAmount = 2 },
            new AccessoryMaterialFurnitureUnit() { Id = 2, AccessoryId = new Guid("9a0124ac-6b7d-4eb7-89fa-a4a874659dad"), FurnitureUnitId = new Guid("ebf87e4c-50b8-402c-b694-ad29183e0bb1"), Name = "Acc-Mat-Name-002", AccessoryAmount = 1 },
            new AccessoryMaterialFurnitureUnit() { Id = 3, AccessoryId = new Guid("695bf260-ae0b-47f1-b4c5-3adba851b694"), FurnitureUnitId = new Guid("4105025C-E947-4D82-8E72-216582EC6B94"), Name = "Acc-Mat-Name-003", AccessoryAmount = 4 },
            new AccessoryMaterialFurnitureUnit() { Id = 4, AccessoryId = new Guid("9a0124ac-6b7d-4eb7-89fa-a4a874659dad"), FurnitureUnitId = new Guid("4105025C-E947-4D82-8E72-216582EC6B94"), Name = "Acc-Mat-Name-004", AccessoryAmount = 3 }
        };
        //public AccessoryMaterialFurnitureUnit[] Entities => new AccessoryMaterialFurnitureUnit[] { };
    }
}
