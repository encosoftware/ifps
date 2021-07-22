using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class OfferGeneralInformationDetailsDto
    {
        public RequiresDetailsDto Requires { get; set; }
        public CabinetMaterialDetailsDto TopCabinet { get; set; }
        public CabinetMaterialDetailsDto BaseCabinet { get; set; }
        public CabinetMaterialDetailsDto TallCabinet { get; set; }

        public OfferGeneralInformationDetailsDto(Order order)
        {
            Requires = new RequiresDetailsDto(order);

            if(order.TopCabinet != null)
            {
                TopCabinet = new CabinetMaterialDetailsDto(order.TopCabinet);
            }
            if(order.BaseCabinet != null)
            {
                BaseCabinet = new CabinetMaterialDetailsDto(order.BaseCabinet);
            }
            if (order.TallCabinet != null)
            {
                TallCabinet = new CabinetMaterialDetailsDto(order.TallCabinet);
            }
        }
    }
}
