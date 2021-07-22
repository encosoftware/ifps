using ENCO.DDD.Domain.Model.Entities.Auditing;


namespace IFPS.Sales.Domain.Model
{
    public class CustomerNotificationMode : FullAuditedEntity
    {
        public virtual Customer Customer{ get; set; }
        public int CustomerId { get; set; }

        public virtual EventType EventType { get; set; }
        public int EventTypeId { get; set; }

        public CustomerNotificationMode(int eventTypeId, int customerId)
        {
            this.CustomerId = customerId;
            this.EventTypeId = eventTypeId;
        }

    }
}
