using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class AccessoryMaterialDetailsDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ImageDetailsDto Image { get; set; }
        public bool IsOptional { get; set; }
        public bool IsRequiredForAssembly { get; set; }
        public PriceDetailsDto Price { get; set; }
        public int TransactionMultiplier { get; set; }

        public AccessoryMaterialDetailsDto(AccessoryMaterial accessoryMaterial)
        {
            Id = accessoryMaterial.Id;
            Code = accessoryMaterial.Code;
            Description = accessoryMaterial.Description;
            CategoryId = accessoryMaterial.CategoryId.Value;
            Image = new ImageDetailsDto(accessoryMaterial.Image);
            IsOptional = accessoryMaterial.IsOptional;
            IsRequiredForAssembly = accessoryMaterial.IsRequiredForAssembly;
            Price = new PriceDetailsDto(accessoryMaterial.CurrentPrice.Price);
            TransactionMultiplier = accessoryMaterial.TransactionMultiplier;
        }
    }
}
