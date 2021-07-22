using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class VenueFilterDto : OrderedPagedRequestDto
    {
        public string Name { get; set; }

        public int Rooms { get; set; }

        public string OfficeAddress { get; set; }

        public string PhoneNUmber { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(Name), nameof(Venue.Name) },
                { nameof(Email), nameof(Venue.Email) },
                { nameof(PhoneNUmber), nameof(Venue.PhoneNumber) },
                { nameof(OfficeAddress), nameof(Venue.OfficeAddress) },
                { nameof(Rooms), nameof(Venue.MeetingRooms)}
            };
        }
    }
}