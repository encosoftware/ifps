using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FoilMaterialUpdateDto
    {
        public string Description { get; set; }
        public ImageUpdateDto ImageUpdateDto { get; set; }
        public double Thickness { get; set; }
        public PriceUpdateDto PriceUpdateDto { get; set; }
        public int TransactionMultiplier { get; set; }

        public FoilMaterial UpdateModelObject(FoilMaterial foilMaterial)
        {
            foilMaterial.Description = Description;
            foilMaterial.Thickness = Thickness;
            foilMaterial.TransactionMultiplier = TransactionMultiplier;
            if (foilMaterial.CurrentPrice.Price != PriceUpdateDto.CreateModelObject())
            {
                foilMaterial.CurrentPrice.ValidTo = DateTime.Now;
                foilMaterial.AddPrice(new MaterialPrice(foilMaterial.Id, PriceUpdateDto.CreateModelObject()));
            }
            return foilMaterial;
        }
    }
}
