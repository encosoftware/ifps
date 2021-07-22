using IFPS.Sales.Domain.Model;
using System;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class MeetingRoomAppointmentsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CurrentlyBooked { get; set; }
        public string Venue { get; set; }
        public AddressDetailsDto Address { get; set; }

        public MeetingRoomAppointmentsDto(MeetingRoom meetingRoom)
        {
            Id = meetingRoom.Id;
            Name = meetingRoom.Name;
            CurrentlyBooked = meetingRoom.Appointments.Any(ent => ent.ScheduledDateTime.From < Clock.Now && ent.ScheduledDateTime.To > Clock.Now);
            Venue = meetingRoom.Venue.Name;
            Address = new AddressDetailsDto(meetingRoom.Venue.OfficeAddress);
        }
    }
}
