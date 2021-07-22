using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class ApplianceUpdateByOfferDto
    {
        public int Quantity { get; set; }

        public ApplianceUpdateByOfferDto()
        {

        }

        public OrderedApplianceMaterial UpdateModelObject(OrderedApplianceMaterial orderedAppliance)
        {
            orderedAppliance.Quantity = Quantity;

            return orderedAppliance;
        }
    }
}
