using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class AccessoryFurnitureUnitDetailsDto
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public Guid MaterialId { get; set; }
        public ImageThumbnailDetailsDto ImageThumbnailDetailsDto { get; set; }

        public AccessoryFurnitureUnitDetailsDto(AccessoryMaterialFurnitureUnit accessoryMaterialFurnitureUnit)
        {
            Name = accessoryMaterialFurnitureUnit.Name;
            Amount = accessoryMaterialFurnitureUnit.AccessoryAmount;
            MaterialId = accessoryMaterialFurnitureUnit.AccessoryId;
            ImageThumbnailDetailsDto = new ImageThumbnailDetailsDto(accessoryMaterialFurnitureUnit.Accessory.Image);
        }
    }
}
