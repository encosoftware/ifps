using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class ShippingServiceListByOfferDto
    {
        public string Description { get; set; }
        public PriceListDto BasicFee { get; set; }
        public int DistanceServiceId { get; set; }
        public PriceListDto Total { get; set; }

        public ShippingServiceListByOfferDto(OrderedService orderedService, double shippingBasicfee)
        {
            Description = orderedService.Service.Description;
            BasicFee = new PriceListDto(shippingBasicfee,
                orderedService.Service.CurrentPrice.Price.CurrencyId,
                orderedService.Service.CurrentPrice.Price.Currency.Name);

            DistanceServiceId = orderedService.ServiceId;
            Total = new PriceListDto(orderedService.Service.CurrentPrice.Price);
        }
    }
}
