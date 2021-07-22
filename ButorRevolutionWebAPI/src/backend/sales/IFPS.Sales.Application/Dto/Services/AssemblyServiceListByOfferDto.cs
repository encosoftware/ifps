using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class AssemblyServiceListByOfferDto
    {
        public string Description { get; set; }
        public int AssemblyServiceId { get; set; }
        public PriceListDto Price { get; set; }

        public AssemblyServiceListByOfferDto(OrderedService orderedService)
        {
            Description = orderedService.Service.Description;
            AssemblyServiceId = orderedService.Service.ServiceTypeId;
            if (orderedService.Service.CurrentPrice != null)
            {
                Price = new PriceListDto(orderedService.Service.CurrentPrice.Price);
            }
        }
    }
}
