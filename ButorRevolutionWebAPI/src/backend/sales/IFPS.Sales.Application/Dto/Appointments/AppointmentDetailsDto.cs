using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class AppointmentDetailsDto
    {
        public string Subject { get; set; }
        public int CategoryId { get; set; }
        public int CustomerId { get; set; }
        public Guid? OrderId { get; set; }
        public string CustomerName { get; set; }
        public int PartnerId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public AddressDetailsDto Address { get; set; }
        public int? VenueId { get; set; }
        public int MeetingRoomId { get; set; }
        public string Notes { get; set; }

        public AppointmentDetailsDto()
        {

        }
        public AppointmentDetailsDto(Appointment appointment)
        {
            Subject = appointment.Subject;
            CategoryId = appointment.CategoryId.GetValueOrDefault();
            CustomerId = appointment.CustomerId.GetValueOrDefault();
            CustomerName = appointment.Customer.CurrentVersion.Name;
            OrderId = appointment.OrderId;
            PartnerId = appointment.PartnerId;
            From = appointment.ScheduledDateTime.From;
            To = appointment.ScheduledDateTime.To;
            Address = appointment.MeetingRoomId == null ? new AddressDetailsDto(appointment.Address) : new AddressDetailsDto(appointment.MeetingRoom.Venue.OfficeAddress);
            VenueId = appointment.MeetingRoom?.VenueId;
            MeetingRoomId = appointment.MeetingRoomId.GetValueOrDefault();
            Notes = appointment.Notes;
        }
    }
}
