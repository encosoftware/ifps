using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class AccessoryMaterialUpdateDto
    {
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ImageUpdateDto ImageUpdateDto { get; set; }
        public PriceUpdateDto PriceUpdateDto { get; set; }
        public int TransactionMultiplier { get; set; }

        public AccessoryMaterial UpdateModelObject(AccessoryMaterial accessoryMaterial)
        {
            accessoryMaterial.Description = Description;
            accessoryMaterial.CategoryId = CategoryId;
            accessoryMaterial.TransactionMultiplier = TransactionMultiplier;
            if (accessoryMaterial.CurrentPrice.Price != PriceUpdateDto.CreateModelObject())
            {
                accessoryMaterial.CurrentPrice.ValidTo = DateTime.Now;
                accessoryMaterial.AddPrice(new MaterialPrice(accessoryMaterial.Id, PriceUpdateDto.CreateModelObject()));
            }
            return accessoryMaterial;
        }
    }
}
