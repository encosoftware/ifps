using IFPS.Sales.Domain.Enums;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class AbsenceDayDto
    {
        public DateTime Date { get; set; }
        public AbsenceTypeEnum AbsenceType { get; set; }
    }
}
