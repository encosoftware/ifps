using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class AccessoryFurnitureUnitCreateDto
    {
        public Guid FurnitureUnitId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public Guid MaterialId { get; set; }

        public AccessoryMaterialFurnitureUnit CreateModelObject()
        {
            return new AccessoryMaterialFurnitureUnit(Name, Amount)
            {
                FurnitureUnitId = FurnitureUnitId,
                AccessoryId = MaterialId
            };
        }
    }
}
