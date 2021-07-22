using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class OfferDetailsDto
    {
        public OfferGeneralInformationDetailsDto GeneralInformations { get; set; }

        public OfferProductsDetailsDto Products { get; set; }

        public OfferDetailsDto(Order order, double shippingBasicFee, double installationBasicFee)
        {
            GeneralInformations = new OfferGeneralInformationDetailsDto(order);
            Products = new OfferProductsDetailsDto(order, shippingBasicFee, installationBasicFee);
        }
    }
}
