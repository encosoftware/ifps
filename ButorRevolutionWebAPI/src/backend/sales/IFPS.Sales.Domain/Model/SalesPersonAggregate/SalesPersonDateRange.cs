using ENCO.DDD.Domain.Model.Entities;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class SalesPersonDateRange : Entity
    {
        public virtual SalesPerson SalesPerson { get; set; }
        public int SalesPersonId { get; set; }

        public DayType DayType { get; private set; }
        public int DayTypeId { get; private set; }

        public virtual DateRange Interval { get; set; }

        public SalesPersonDateRange(int dayTypeId, DateTime from, DateTime to, int salesPresonId)
        {
            this.DayTypeId = dayTypeId;
            this.Interval = new DateRange(from, to);
            this.SalesPersonId = salesPresonId;
        }

        public SalesPersonDateRange()
        {

        }
    }
}
