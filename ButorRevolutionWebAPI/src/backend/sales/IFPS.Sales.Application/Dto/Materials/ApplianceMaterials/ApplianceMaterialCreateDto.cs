using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class ApplianceMaterialCreateDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ImageCreateDto ImageCreateDto { get; set; }
        public int CompanyId { get; set; }
        public string HanaCode { get; set; }
        public PriceCreateDto PurchasingPrice { get; set; }
        public PriceCreateDto SellPrice { get; set; }

        public ApplianceMaterial CreateModelObject()
        {
            return new ApplianceMaterial(Code)
            {
                Description = Description,
                CategoryId = CategoryId,
                BrandId = CompanyId,
                HanaCode = HanaCode,
                SellPrice = SellPrice.CreateModelObject(),
            };
        }
    }
}
