using IFPS.Sales.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class VenueUpdateDto
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public AddressUpdateDto OfficeAddress { get; set; }

        public List<VenueDateRangeUpdateDto> OpeningHours { get; set; }

        public List<MeetingRoomUpdateDto> MeetingRooms { get; set; }

        public VenueUpdateDto()
        {

        }

        public Venue UpdateModelObject(Venue venue)
        {
            venue.Name = Name;
            venue.PhoneNumber = PhoneNumber;
            venue.Email = Email;
            venue.OfficeAddress = OfficeAddress.CreateModelObject();

            return venue;
        }
    }
}
