using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class ApplianceDetailsByOfferDto
    {
        public int Quantity { get; set; }

        public ApplianceDetailsByOfferDto(OrderedApplianceMaterial orderedAppliance)
        {
            Quantity = orderedAppliance.Quantity;
        }
    }
}
