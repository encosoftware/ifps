using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class ApplianceMaterialListDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public CategoryListDto Category { get; set; }
        public ImageThumbnailListDto Image { get; set; }
        public string Brand { get; set; }
        public string HanaCode { get; set; }
        public PriceListDto PurchasingPrice { get; set; }
        public PriceListDto SellPrice { get; set; }

        public ApplianceMaterialListDto(ApplianceMaterial applianceMaterial)
        {
            Id = applianceMaterial.Id;
            Code = applianceMaterial.Code;
            Description = applianceMaterial.Description;
            Category = new CategoryListDto(applianceMaterial.Category);
            Image = new ImageThumbnailListDto(applianceMaterial.Image);
            Brand = applianceMaterial.Brand.Name;
            HanaCode = applianceMaterial.HanaCode;
            PurchasingPrice = new PriceListDto(applianceMaterial.CurrentPrice.Price);
            SellPrice = new PriceListDto(applianceMaterial.SellPrice);
        }

        public ApplianceMaterialListDto() { }

        public static Func<ApplianceMaterial, ApplianceMaterialListDto> FromEntity = entity => new ApplianceMaterialListDto(entity);
    }
}
