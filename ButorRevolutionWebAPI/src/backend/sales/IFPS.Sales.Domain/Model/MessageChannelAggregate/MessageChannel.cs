using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Domain.Model
{
    public class MessageChannel : FullAuditedAggregateRoot
    {
        public virtual Order Order { get; set; }
        public Guid OrderId { get; set; }

        /// <summary>
        /// Private list of the participants of the channel
        /// </summary>
        private List<MessageChannelParticipant> _participants;

        public IEnumerable<MessageChannelParticipant> Participants => _participants.AsReadOnly();


        private List<string> _connectionIds;
        public IEnumerable<string> ConnectionIds => _connectionIds.AsReadOnly();

        /// <summary>
        /// Private list of the messages of the channel
        /// </summary>
        private List<Message> _messages;

        public IEnumerable<Message> Messages => _messages.AsReadOnly();

        private MessageChannel()
        {
            _participants = new List<MessageChannelParticipant>();
            _messages = new List<Message>();
            _connectionIds = new List<string>();
        }

        public MessageChannel(Guid orderId) : this()
        {
            OrderId = orderId;
        }

        public void AddMessageChannelParticipant(MessageChannelParticipant messageChannelParticipant)
        {
            Ensure.NotNull(messageChannelParticipant);

            _participants.Add(messageChannelParticipant);
        }
    }
}
