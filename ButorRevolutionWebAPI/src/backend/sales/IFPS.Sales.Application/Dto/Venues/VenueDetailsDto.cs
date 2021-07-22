using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class VenueDetailsDto
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public AddressDetailsDto OfficeAddress { get; set; }

        public List<MeetingRoomsListDto> MeetingRooms { get; set; }

        public List<VenueDateRangeListDto> OpeningHours { get; set; }

        public bool IsActive { get; set; }

        public VenueDetailsDto(Venue venue)
        {
            Name = venue.Name;
            Email = venue.Email;
            OfficeAddress = new AddressDetailsDto(venue.OfficeAddress);
            PhoneNumber = venue.PhoneNumber;
            OpeningHours = venue.OpeningHours.Select(entity => new VenueDateRangeListDto(entity)).ToList();
            MeetingRooms = venue.MeetingRooms.Select(entity => new MeetingRoomsListDto(entity)).ToList();
            IsActive = venue.IsActive;
        }
    }
}
