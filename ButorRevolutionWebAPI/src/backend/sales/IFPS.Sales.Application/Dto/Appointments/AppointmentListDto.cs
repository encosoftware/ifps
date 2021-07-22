using IFPS.Sales.Domain.Model;
using System;
using IFPS.Sales.Domain.Dbos;

namespace IFPS.Sales.Application.Dto
{
    public class AppointmentListDto
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Subject { get; set; }
        public string CustomerName { get; set; }
        public AddressDetailsDto Address { get; set; }
        public string Notes { get; set; }
        public string CategoryName { get; set; }

        public AppointmentListDto()
        {

        }

        public AppointmentListDto(AppointmentListDbo appointmentDbo)
        {
            Id = appointmentDbo.Id;
            From = appointmentDbo.From;
            To = appointmentDbo.To;
            Subject = appointmentDbo.Subject;
            CustomerName = appointmentDbo.CustomerName;
            Address = appointmentDbo.MeetingRoomId == null ? new AddressDetailsDto(appointmentDbo.Address) : new AddressDetailsDto(appointmentDbo.OfficeAddress);
            Notes = appointmentDbo.Notes;
            CategoryName = appointmentDbo.CategoryName;
        }

        public AppointmentListDto(Appointment appointment)
        {
            Id = appointment.Id;
            From = appointment.ScheduledDateTime.From;
            To = appointment.ScheduledDateTime.To;
            Subject = appointment.Subject;
            CustomerName = appointment.Customer.CurrentVersion.Name;
            Address = appointment.MeetingRoomId == null ? new AddressDetailsDto(appointment.Address) : new AddressDetailsDto(appointment.MeetingRoom.Venue.OfficeAddress);
            Notes = appointment.Notes;
            CategoryName = appointment.Category.CurrentTranslation.GroupingCategoryName;
        }       
    }
}
