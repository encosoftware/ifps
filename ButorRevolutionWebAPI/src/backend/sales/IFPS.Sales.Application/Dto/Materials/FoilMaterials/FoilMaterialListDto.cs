using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FoilMaterialListDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public ImageThumbnailListDto Image { get; set; }
        public double Thickness { get; set; }
        public PriceListDto Price { get; set; }
        public int TransactionMultiplier { get; set; }

        public FoilMaterialListDto(FoilMaterial foilMaterial)
        {
            Id = foilMaterial.Id;
            Code = foilMaterial.Code;
            Description = foilMaterial.Description;
            Image = new ImageThumbnailListDto(foilMaterial.Image);
            Thickness = foilMaterial.Thickness;
            Price = new PriceListDto(foilMaterial.CurrentPrice.Price);
            TransactionMultiplier = foilMaterial.TransactionMultiplier;
        }

        public FoilMaterialListDto() { }

        public static Func<FoilMaterial, FoilMaterialListDto> FromEntity = entity => new FoilMaterialListDto(entity);
    }
}
