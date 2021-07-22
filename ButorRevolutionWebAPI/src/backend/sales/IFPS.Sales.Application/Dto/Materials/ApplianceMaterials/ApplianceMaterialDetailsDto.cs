using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class ApplianceMaterialDetailsDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ImageDetailsDto Image { get; set; }
        public int CompanyId { get; set; }
        public string HanaCode { get; set; }
        public PriceDetailsDto PurchasingPrice { get; set; }
        public PriceDetailsDto SellPrice { get; set; }

        public ApplianceMaterialDetailsDto(ApplianceMaterial applianceMaterial)
        {
            Id = applianceMaterial.Id;
            Code = applianceMaterial.Code;
            Description = applianceMaterial.Description;
            CategoryId = applianceMaterial.CategoryId.Value;
            Image = new ImageDetailsDto(applianceMaterial.Image);
            CompanyId = applianceMaterial.BrandId;
            HanaCode = applianceMaterial.HanaCode;
            PurchasingPrice = new PriceDetailsDto(applianceMaterial.CurrentPrice.Price);
            SellPrice = new PriceDetailsDto(applianceMaterial.SellPrice);
        }
    }
}
