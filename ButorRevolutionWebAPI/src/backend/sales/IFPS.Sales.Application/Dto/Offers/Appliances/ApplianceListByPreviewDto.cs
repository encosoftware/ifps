using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class ApplianceListByPreviewDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public PriceListDto Price { get; set; }
        public int Quantity { get; set; }
        public ImageThumbnailDetailsDto Image { get; set; }
        public PriceListDto SubTotal { get; set; }

        public ApplianceListByPreviewDto(OrderedApplianceMaterial orderedAppliance)
        {
            var price = orderedAppliance.ApplianceMaterial.SellPrice;

            Code = orderedAppliance.ApplianceMaterial.Code;
            Name = orderedAppliance.ApplianceMaterial.Description;
            Price = new PriceListDto(price);
            Image = new ImageThumbnailDetailsDto(orderedAppliance.ApplianceMaterial.Image);
            Quantity = orderedAppliance.Quantity;
            SubTotal = new PriceListDto(price, Quantity);
        }
    }
}
