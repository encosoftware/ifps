using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class InstallationServiceListByOfferDto
    {
        public string Description { get; set; }
        public int InstallationServiceId { get; set; }
        public PriceListDto InstallationBasicFee { get; set; }
        public PriceListDto Price { get; set; }

        public InstallationServiceListByOfferDto(OrderedService orderedService, double installationBasicFee)
        {
            Description = orderedService.Service.Description;
            InstallationServiceId = orderedService.Service.ServiceTypeId;
            //Price = new PriceListDto(orderedService.Service.CurrentPrice.Price);
            InstallationBasicFee = new PriceListDto(installationBasicFee, orderedService.Order.Budget.CurrencyId, orderedService.Order.Budget.Currency.Name);
        }
    }
}
