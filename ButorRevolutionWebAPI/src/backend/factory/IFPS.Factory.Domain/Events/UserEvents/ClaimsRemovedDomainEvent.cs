using System.Collections.Generic;

namespace IFPS.Factory.Domain.Events.User
{
    public class ClaimsRemovedDomainEvent : IIFPSDomainEvent
    {
        public List<int> RemovedClaimsIds { get; set; }
        public int EditedUserId { get; set; }
        public int? InitiatorUserId { get; set; }
    }
}
