using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class AccessoryFurnitureUnitListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Code { get; set; }
        public ImageThumbnailListDto ImageThumbnailListDto { get; set; }

        public AccessoryFurnitureUnitListDto(AccessoryMaterialFurnitureUnit accessoryMaterialFurnitureUnit)
        {
            Id = accessoryMaterialFurnitureUnit.Id;
            Name = accessoryMaterialFurnitureUnit.Name;
            Amount = accessoryMaterialFurnitureUnit.AccessoryAmount;
            Code = accessoryMaterialFurnitureUnit.Accessory.Code;
            ImageThumbnailListDto = new ImageThumbnailListDto(accessoryMaterialFurnitureUnit.Accessory.Image);
        }
    }
}
