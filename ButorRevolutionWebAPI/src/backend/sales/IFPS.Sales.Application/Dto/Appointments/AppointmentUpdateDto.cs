using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class AppointmentUpdateDto
    {
        public string Subject { get; set; }
        public int CategoryId { get; set; }
        public int CustomerId { get; set; }
        public int PartnerId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public AddressUpdateDto Address { get; set; }
        public int? MeetingRoomId { get; set; }
        public string Notes { get; set; }

        public Appointment UpdateModelObject(Appointment appointment)
        {
            appointment.Subject = Subject;
            appointment.CategoryId = CategoryId;
            appointment.CustomerId = CustomerId;
            appointment.PartnerId = PartnerId;
            appointment.ScheduledDateTime = new DateRange(From, To);
            if(Address != null)
            {
                appointment.Address = Address.CreateModelObject();
            }            
            appointment.MeetingRoomId = MeetingRoomId;
            appointment.Notes = Notes;
            return appointment;
        }
    }
}
