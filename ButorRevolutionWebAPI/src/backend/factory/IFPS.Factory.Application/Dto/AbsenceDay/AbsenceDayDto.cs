using IFPS.Factory.Domain.Enums;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class AbsenceDayDto
    {
        public DateTime Date { get; set; }
        public AbsenceTypeEnum AbsenceType { get; set; }
    }
}