using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Domain.Model
{
    public class Message : FullAuditedAggregateRoot
    {
        /// <summary>
        /// Text of the message
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Timestamp of the sent message
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// User, who sent the message
        /// </summary>
        public MessageChannelParticipant Sender { get; set; }
        public int SenderId { get; set; }

        /// <summary>
        /// MessageChannel, which receives the message
        /// </summary>
        public MessageChannel MessageChannel { get; set; }
        public int MessageChannelId { get; set; }

        ///// <summary>
        ///// Private list of the message recipients
        ///// </summary>
        //private List<ParticipantMessage> _recipients;

        //public IEnumerable<ParticipantMessage> Recipients => _recipients.AsReadOnly();

        private Message()
        {
        }

        public Message(int messageChannelId, int senderId, string text)
        {
            Text = text;
            MessageChannelId = messageChannelId;
            SenderId = senderId;
        }
    }
}
