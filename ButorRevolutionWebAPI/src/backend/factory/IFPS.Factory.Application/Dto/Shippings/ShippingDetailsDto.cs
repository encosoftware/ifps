using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class ShippingDetailsDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public PriceDetailsDto ShippingCost { get; set; }
        public AddressDetailsDto ShippingAddress { get; set; }
        public string Note { get; set; }

        public ShippingDetailsDto(Cargo cargo)
        {
            Name = cargo.BookedBy.CurrentVersion.Name;
            Phone = cargo.BookedBy.CurrentVersion.Phone;
            ShippingCost = new PriceDetailsDto(cargo.ShippingCost);
            ShippingAddress = new AddressDetailsDto(cargo.ShippingAddress);
            Note = cargo.Notes;
        }
    }
}
