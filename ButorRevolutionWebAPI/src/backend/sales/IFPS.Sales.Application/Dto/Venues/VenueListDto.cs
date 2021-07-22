using IFPS.Sales.Domain.Model;
using System;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class VenueListDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Rooms { get; set; }

        public AddressDetailsDto Address { get; set; }

        public string PhoneNumber { get; set; }
        
        public string Email { get; set; }

        public bool IsAktiv { get; set; }

        public VenueListDto() { }
        
        public static Func<Venue, VenueListDto> FromEntity = entity => new VenueListDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Rooms = entity.MeetingRooms.Count(),
            Address = new AddressDetailsDto(entity.OfficeAddress),
            PhoneNumber = entity.PhoneNumber,
            Email = entity.Email,
            IsAktiv = entity.IsActive
        };
    }
}
