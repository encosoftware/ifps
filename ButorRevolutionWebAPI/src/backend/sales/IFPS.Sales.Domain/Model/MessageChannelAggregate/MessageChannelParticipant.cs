using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Domain.Model
{
    public class MessageChannelParticipant : FullAuditedAggregateRoot
    {
        /// <summary>
        /// Message channel, which receives the message
        /// </summary>
        public virtual MessageChannel MessageChannel { get; set; }
        public int MessageChannelId { get; set; }

        /// <summary>
        /// User, who receives the messages through the channel
        /// </summary>
        public virtual User User { get; set; }
        public int UserId { get; set; }

        /// <summary>
        /// Private list of the messages of the participant
        /// </summary>
        private List<ParticipantMessage> _participantMessages;
        public IEnumerable<ParticipantMessage> ParticipantMessages => _participantMessages.AsReadOnly();

        /// <summary>
        /// Timestamp, when the user entered the channel
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Timestamp, when the user left the channel
        /// </summary>
        public DateTime? ValidTo { get; set; }

        private MessageChannelParticipant()
        {
            _participantMessages = new List<ParticipantMessage>();
        }

        public MessageChannelParticipant(int messageChannelId, int participantId) : this()
        {
            MessageChannelId = messageChannelId;
            UserId = participantId;
        }
    }
}
