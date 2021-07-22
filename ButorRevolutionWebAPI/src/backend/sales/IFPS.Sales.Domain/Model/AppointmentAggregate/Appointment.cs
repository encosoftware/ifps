using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class Appointment : FullAuditedAggregateRoot
    {
        /// <summary>
        /// Time and date of the appointment
        /// </summary>
        public DateRange ScheduledDateTime { get; set; }

        /// <summary>
        /// Customer, who is invited to the appointment
        /// </summary>
        public virtual User Customer { get; set; }
        public int? CustomerId { get; set; }

        /// <summary>
        /// There is an opportunity to create an appointment for person who has no account yet.
        /// </summary>
        public virtual AnonymousUserData AnonymousUser { get; set; }
        public int? AnonymousUserId { get; set; }

        /// <summary>
        /// Salesperson or partner, who is invited to the appointment
        /// </summary>
        public virtual User Partner { get; set; }
        public int PartnerId { get; set; }

        /// <summary>
        /// Order which is connected to the appointment
        /// </summary>
        public virtual Order Order { get; set; }
        public Guid? OrderId { get; set; }

        /// <summary>
        /// If the appointment is held in a meeting room of the office building
        /// </summary>
        public virtual MeetingRoom MeetingRoom { get; set; }
        public int? MeetingRoomId { get; set; }

        /// <summary>
        /// Address of the meeting, If it is held outside of the office
        /// </summary>
        public virtual Address Address { get; set; }


        /// <summary>
        /// Category the appointment belongs to
        /// </summary>
        public virtual GroupingCategory Category { get; set; }
        public int? CategoryId { get; set; }

        /// <summary>
        /// Subject of the appointment. Optional
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// True, if the appointment was cancelled
        /// </summary>
        public bool IsCanceled { get; private set; }

        /// <summary>
        /// User, who canceled the appointment
        /// </summary>
        public virtual User CanceledBy { get; set; }
        public int? CanceledById { get; set; }

        /// <summary>
        /// True, if this appointment is generated from the system. 
        /// False, if this appointment is synchronized from the google calendar
        /// </summary>
        public bool IsFrameworkAppointment { get; private set; }

        /// <summary>
        /// Notes associated with the appointment
        /// </summary>
        public string Notes { get; set; }

        private Appointment()
        {

        }

        public Appointment(DateRange scheduledDateTime, int partnerId, string notes) : this()
        {
            ScheduledDateTime = scheduledDateTime;
            PartnerId = partnerId;
            Notes = notes;
        }
    }
}
