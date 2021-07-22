using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace IFPS.Sales.Domain.Dbos
{
    public class MessageChannelListDbo
    {
        public int ParticipantUserId { get; set; }
        public Message LastSentMessage { get; set; }
        public IEnumerable<int> ContactUserIds { get; set; }
        public int MessageChannelId { get; set; }

        public static Expression<Func<MessageChannelParticipant, MessageChannelListDbo>> Projection
        {
            get
            {
                return x => new MessageChannelListDbo
                {
                    ParticipantUserId = x.UserId,
                    MessageChannelId = x.MessageChannelId,
                    LastSentMessage = x.MessageChannel.Messages.OrderByDescending(m => m.TimeStamp).FirstOrDefault(),
                    ContactUserIds = x.MessageChannel.Participants.Select(ent => ent.UserId)
                };
            }
        }
    }
}
