using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class VenueCreateDto
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public AddressCreateDto OfficeAddress { get; set; }

        public VenueCreateDto()
        {
        }

        public Venue CreateVenue()
        {
            return new Venue(Name, PhoneNumber, Email, OfficeAddress.CreateModelObject());
        }
    }
}