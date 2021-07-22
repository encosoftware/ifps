using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Sales.Domain.Model
{
    public class CompanyDateRange : FullAuditedEntity
    {
        /// <summary>
        /// Day type
        /// </summary>
        public DayType DayType { get; private set; }
        public int DayTypeId { get; private set; }

        /// <summary>
        /// Company entity
        /// </summary>
        public virtual Company Company { get; private set; }
        public int CompanyId { get; private set; }

        /// <summary>
        /// Default date range, when the company awaits clients
        /// </summary>
        public DateRange Interval { get; private set; }

        private CompanyDateRange() { }

        public CompanyDateRange(Company company, DateRange interval, int dayTypeId)
        {
            Company = company;
            Interval = interval;
            DayTypeId = dayTypeId;
        }
    }
}
