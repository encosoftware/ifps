using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class ShippingFromCargoDetailsDto
    {
        public PriceListDto ShippingCost { get; set; }

        public AddressDetailsDto ShippingAddress { get; set; }

        public string Note { get; set; }

        public ShippingFromCargoDetailsDto(Cargo cargo)
        {
            ShippingCost = new PriceListDto(cargo.ShippingCost);
            ShippingAddress = new AddressDetailsDto(cargo.ShippingAddress);
            Note = cargo.Notes;
        }
    }
}
