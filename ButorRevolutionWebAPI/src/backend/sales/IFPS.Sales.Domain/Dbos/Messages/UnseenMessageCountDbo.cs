using IFPS.Sales.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace IFPS.Sales.Domain.Dbos
{
    public class UnseenMessageCountDbo
    {
        public int UserId { get; set; }
        public int MessageChannelId { get; set; }
        public int MessageCount { get; set; }

        public static Expression<Func<MessageChannelParticipant, UnseenMessageCountDbo>> Projection
        {
            get
            {
                return x => new UnseenMessageCountDbo
                {
                    UserId = x.UserId,
                    MessageChannelId = x.MessageChannelId,
                    MessageCount = x.ParticipantMessages.Count(pm => !pm.Seen),
                };
            }
        }
    }
}
