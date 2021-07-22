using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class ApplianceMaterialUpdateDto
    {
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ImageUpdateDto ImageUpdateDto { get; set; }
        public int CompanyId { get; set; }
        public string HanaCode { get; set; }
        public PriceUpdateDto PurchasingPrice { get; set; }
        public PriceUpdateDto SellPrice { get; set; }

        public ApplianceMaterial UpdateModelObject(ApplianceMaterial applianceMaterial)
        {
            applianceMaterial.Description = Description;
            applianceMaterial.CategoryId = CategoryId;
            applianceMaterial.BrandId = CompanyId;
            applianceMaterial.HanaCode = HanaCode;
            if (applianceMaterial.CurrentPrice.Price != PurchasingPrice.CreateModelObject())
            {
                applianceMaterial.CurrentPrice.ValidTo = DateTime.Now;
                applianceMaterial.AddPrice(new MaterialPrice(applianceMaterial.Id, PurchasingPrice.CreateModelObject()));
            }
            if (applianceMaterial.SellPrice !=  SellPrice.CreateModelObject() )
            {
                applianceMaterial.SellPrice = SellPrice.CreateModelObject();
            }
            return applianceMaterial;
        }
    }
}
