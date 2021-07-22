using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class UserNotificationText : FullAuditedAggregateRoot
    {
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Notification text sent to the user
        /// </summary>
        public string Text { get; set; }        

        public DateTime? Sent { get; set; }
    }
}
