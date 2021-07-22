using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class AccessoryFurnitureUnitUpdateDto
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public Guid MaterialId { get; set; }
        public ImageUpdateDto ImageUpdateDto { get; set; }

        public AccessoryMaterialFurnitureUnit UpdateModelObject(AccessoryMaterialFurnitureUnit furnitureComponent)
        {
            furnitureComponent.Name = Name;
            furnitureComponent.AccessoryAmount = Amount;
            furnitureComponent.AccessoryId = MaterialId;
            return furnitureComponent;
        }
    }
}
