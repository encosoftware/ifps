using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Factory.Domain.Model
{
    public class VenueDateRange : AuditedEntity
    {
        public Venue Venue { get; set; }
        public int VenueId { get; set; }

        /// <summary>
        /// Date type
        /// </summary>
        public DayType DayType { get; set; }

        public int DayTypeId { get; private set; }

        /// <summary>
        /// Opening and closing hours of the venue
        /// </summary>
        public DateRange Interval { get; private set; }

        private VenueDateRange()
        { }

        public VenueDateRange(int venueId, int dayTypeId, DateRange interval)
        {
            this.VenueId = venueId;
            this.DayTypeId = dayTypeId;
            this.Interval = interval;
        }
    }
}