using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FoilMaterialDetailsDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public ImageDetailsDto Image { get; set; }
        public double Thickness { get; set; }
        public PriceDetailsDto Price { get; set; }
        public int TransactionMultiplier { get; set; }

        public FoilMaterialDetailsDto(FoilMaterial foilMaterial)
        {
            Id = foilMaterial.Id;
            Code = foilMaterial.Code;
            Description = foilMaterial.Description;
            Image = new ImageDetailsDto(foilMaterial.Image);
            Thickness = foilMaterial.Thickness;
            Price = new PriceDetailsDto(foilMaterial.CurrentPrice.Price);
            TransactionMultiplier = foilMaterial.TransactionMultiplier;
        }
    }
}
