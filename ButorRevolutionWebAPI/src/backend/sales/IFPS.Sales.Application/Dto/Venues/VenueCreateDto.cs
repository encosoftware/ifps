using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class VenueCreateDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public AddressCreateDto OfficeAddress { get; set; }

        public VenueCreateDto() { }

        public Venue CreateVenue()
        {
            return new Venue(Name, PhoneNumber, Email, CompanyId, OfficeAddress.CreateModelObject());
        }

    }
}
