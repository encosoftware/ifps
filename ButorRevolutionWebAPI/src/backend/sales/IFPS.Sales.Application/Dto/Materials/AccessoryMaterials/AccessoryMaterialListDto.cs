using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class AccessoryMaterialListDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public CategoryListDto Category { get; set; }
        public ImageThumbnailListDto Image { get; set; }
        public bool IsOptional { get; set; }
        public bool IsRequiredForAssembly { get; set; }
        public PriceListDto Price { get; set; }
        public int TransactionMultiplier { get; set; }

        public AccessoryMaterialListDto(AccessoryMaterial accessoryMaterial)
        {
            Id = accessoryMaterial.Id;
            Code = accessoryMaterial.Code;
            Description = accessoryMaterial.Description;
            Category = new CategoryListDto(accessoryMaterial.Category);
            Image = new ImageThumbnailListDto(accessoryMaterial.Image);
            IsOptional = accessoryMaterial.IsOptional;
            IsRequiredForAssembly = accessoryMaterial.IsRequiredForAssembly;
            Price = new PriceListDto(accessoryMaterial.CurrentPrice.Price);
            TransactionMultiplier = accessoryMaterial.TransactionMultiplier;
        }

        public AccessoryMaterialListDto() { }

        public static Func<AccessoryMaterial, AccessoryMaterialListDto> FromEntity = entity => new AccessoryMaterialListDto(entity);
    }
}
