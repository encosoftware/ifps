using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class AppointmentCreateDto
    {
        public string Subject { get; set; }
        public int CategoryId { get; set; }
        public int CustomerId { get; set; }
        public int PartnerId { get; set; }
        public Guid? OrderId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public AddressCreateDto AddressCreateDto { get; set; }
        public int? MeetingRoomId { get; set; }
        public string Notes { get; set; }

        public Appointment CreateModelObject()
        {
            return new Appointment(new DateRange(From, To), PartnerId, Notes)
            {
                Subject = Subject,
                CategoryId = CategoryId,
                CustomerId = CustomerId,
                OrderId = OrderId
            };
        }
    }
}
