using System.Collections.Generic;

namespace IFPS.Factory.Domain.Events.User
{
    public class ClaimsAddedDomainEvent : IIFPSDomainEvent
    {
        public List<int> AddedClaimsIds { get; set; }
        public int EditedUserId { get; set; }
        public int? InitiatorUserId { get; set; }
    }
}
