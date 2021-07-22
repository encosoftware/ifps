using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto.Appointments
{
    public class AppointmentDateRangeDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public DateRange CreateModelObject()
        {
            return new DateRange(From, To);
        }
    }
}
