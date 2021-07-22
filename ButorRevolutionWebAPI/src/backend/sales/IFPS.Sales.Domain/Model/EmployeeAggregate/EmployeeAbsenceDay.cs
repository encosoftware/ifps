using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Enums;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class EmployeeAbsenceDay : FullAuditedEntity
    {
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        public DateTime Date { get; set; }

        public AbsenceTypeEnum AbsenceType { get; set; }        
    }
}
