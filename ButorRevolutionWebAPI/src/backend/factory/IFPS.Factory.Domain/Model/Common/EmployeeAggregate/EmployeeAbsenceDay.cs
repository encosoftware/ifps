using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Factory.Domain.Enums;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class EmployeeAbsenceDay : FullAuditedEntity
    {
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        public DateTime Date { get; set; }

        public AbsenceTypeEnum AbsenceType { get; set; }
    }
}