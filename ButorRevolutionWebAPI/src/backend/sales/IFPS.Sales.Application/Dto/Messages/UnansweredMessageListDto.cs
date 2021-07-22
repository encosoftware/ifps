using IFPS.Sales.Domain.Dbos;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class UnansweredMessageListDto
    {
        public string Text { get; set; }
        public int MessageCount { get; set; }
        public bool MessageSeen { get; set; }
        public DateTime? Sent { get; set; }
        public MessageChannelParticipantListDto Sender { get; set; }
        public int ParticipantUserId { get; set; }
        public int MessageChannelId { get; set; }
        

        public UnansweredMessageListDto()
        {
         
        }

        public UnansweredMessageListDto (
            MessageChannelListDbo messageChannel, 
            UserAvatarDbo avatar,
            UnseenMessageCountDbo messageCountDbo)
        {
            Text = messageChannel.LastSentMessage?.Text;
            Sent = messageChannel.LastSentMessage?.TimeStamp;
            MessageChannelId = messageChannel.MessageChannelId;
            MessageCount = messageCountDbo.MessageCount;
            ParticipantUserId = messageChannel.ParticipantUserId;
            Sender = new MessageChannelParticipantListDto(avatar);
        }

        public UnansweredMessageListDto(ParticipantMessage participantMessage)
        {
            Text = participantMessage.Message.Text;
            MessageSeen = participantMessage.Seen;
            Sent = participantMessage.Message.CreationTime;
            Sender = new MessageChannelParticipantListDto(participantMessage.Message.Sender);
            MessageChannelId = participantMessage.Message.MessageChannelId;
        }
    }
}
