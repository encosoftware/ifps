using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Factory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class Employee : FullAuditedAggregateRoot
    {
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }

        private List<EmployeeAbsenceDay> _absenceDays;
        public IEnumerable<EmployeeAbsenceDay> AbsenceDays => _absenceDays.AsReadOnly();

        public static readonly List<DivisionTypeEnum> EmployeesDivisions = new List<DivisionTypeEnum>()
        {
            DivisionTypeEnum.Admin,
            DivisionTypeEnum.Financial,
            DivisionTypeEnum.Production,
            DivisionTypeEnum.Supply,
            DivisionTypeEnum.Warehouse
        };

        private Employee()
        {
            _absenceDays = new List<EmployeeAbsenceDay>();
            ValidFrom = Clock.Now;
        }

        public Employee(int userId) : this()
        {
            UserId = userId;
        }

        public void Close(DateTime? validTo = null)
        {
            ValidTo = validTo ?? DateTime.Now;
        }

        public void AddOrUpdateAbsenceDay(EmployeeAbsenceDay absenceDay)
        {
            Ensure.NotNull(absenceDay);

            if (_absenceDays.Any(ad => absenceDay.Date.Date == ad.Date.Date))
            {
                var day = _absenceDays.First(ad => absenceDay.Date.Date == ad.Date.Date);
                day.AbsenceType = absenceDay.AbsenceType;
            }
            else
            {
                _absenceDays.Add(absenceDay);
            }
        }

        public void DeleteAbsenceDay(DateTime absenceDayDate)
        {
            _absenceDays.RemoveAll(ad => ad.Date.Date == absenceDayDate.Date);
        }
    }
}