using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class SalesPerson : FullAuditedAggregateRoot
    {
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }

        /// <summary>
        /// Supervisor of the user. Nullable
        /// </summary>
        public virtual SalesPerson Supervisor { get; set; }
        public int? SupervisorId { get; set; }


        /// <summary>
        /// Private list for supervisees
        /// </summary>
        private List<SalesPerson> _supervisees;
        /// <summary>
        /// Public readonly collection for reading _supervisees private list
        /// </summary>
        public IEnumerable<SalesPerson> Supervisees => _supervisees.AsReadOnly();

        private List<SalesPersonOffice> _offices;
        public ICollection<SalesPersonOffice> Offices => _offices.AsReadOnly();

        /// <summary>
        /// Default timetable of the employee
        /// </summary>
        private List<SalesPersonDateRange> _defaultTimeTable;
        public IEnumerable<SalesPersonDateRange> DefaultTimeTable => _defaultTimeTable.AsReadOnly();

        /// <summary>
        /// Minimum possible additional discount percent he/she can give 
        /// </summary>
        public int MinDiscount { get; set; }

        /// <summary>
        /// Maximum possible additional discount percent he/she can give 
        /// </summary>
        public int MaxDiscount { get; set; }


        private SalesPerson()
        {
            this._supervisees = new List<SalesPerson>();
            this._offices = new List<SalesPersonOffice>();
            this._defaultTimeTable = new List<SalesPersonDateRange>();
        }

        public SalesPerson(int userId, DateTime validFrom) : this()
        {
            this.UserId = userId;
            this.ValidFrom = validFrom;
        }


        public void Close(DateTime? validTo = null)
        {
            this.ValidTo = validTo ?? DateTime.Now;
        }

        public void RemoveOffices(ICollection<int> officeIds)
        {
            foreach (var office in _offices.Where(op => officeIds.Any(oId => op.OfficeId == oId)))
            {
                office.IsDeleted = true;
                office.DeletionTime = Clock.Now;
            }
        }

        public void AddOffices(ICollection<int> officeIds)
        {
            _offices.AddRange(
                officeIds.Where(oId => !_offices.Any(op => op.OfficeId == oId && !op.IsDeleted))
                    .Select(oId => new SalesPersonOffice(oId, this.Id)
           ));

        }

        public void RemoveSupervisedSalesPersons(ICollection<int> salesPersonsIds)
        {
            _supervisees.RemoveAll(sp => salesPersonsIds.Any(id => sp.Id == id));
        }

        public void RemoveSupervisedSalesPersons(ICollection<SalesPerson> salesPersons)
        {
            _supervisees.RemoveAll(sp => salesPersons.Any(oldSp => sp.Id == oldSp.Id));
        }

        public void AddSupervisedSalesPersons(ICollection<SalesPerson> salesPersons)
        {
            _supervisees.AddRange(salesPersons);
        }

        public void RemoveDefaultTimeTableEntries(ICollection<int> defaultTimeTableEntriesIds)
        {
            _defaultTimeTable.RemoveAll(dtt => defaultTimeTableEntriesIds.Contains(dtt.Id));
        }

        public void AddDefaultTimeTableEntry(int dayTypeId, DateTime from, DateTime to)
        {
            _defaultTimeTable.Add(new SalesPersonDateRange(dayTypeId, from, to, this.Id));
        }

        public void UpdateDefaultTimeTableEntry(int id, int dayTypeId, DateTime from, DateTime to)
        {
            var defaultTimeTableEntry = _defaultTimeTable.Single(dtt => dtt.Id == id);
            if (defaultTimeTableEntry.DayTypeId != dayTypeId)
                throw new IFPSDomainException($"DayType can not be changed during updating DefaultTimeTableEntry by id({id})!");

            defaultTimeTableEntry.Interval = new DateRange(from, to);
           
        }
    }


}
