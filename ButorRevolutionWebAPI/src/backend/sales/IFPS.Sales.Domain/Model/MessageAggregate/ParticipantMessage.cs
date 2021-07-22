using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Sales.Domain.Model
{
    public class ParticipantMessage : FullAuditedAggregateRoot
    {
        /// <summary>
        /// Sent message
        /// </summary>
        public virtual Message Message { get; set; }
        public int MessageId { get; set; }
        
        /// <summary>
        /// Recipient of the message
        /// </summary>
        public virtual MessageChannelParticipant Recipient { get; set; }
        public int RecipientId { get; set; }

        /// <summary>
        /// Bool flag, which indicates, if the recepient has seen the message
        /// </summary>
        public bool Seen { get; set; }
        
        public ParticipantMessage(int messageId, int recipientId)
        {
            Seen = false;
            MessageId = messageId;
            RecipientId = recipientId;
        }
    }
}
