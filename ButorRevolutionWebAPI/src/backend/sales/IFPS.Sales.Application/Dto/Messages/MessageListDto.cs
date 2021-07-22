using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class MessageListDto
    {
        public string Text { get; set; }
        public DateTime Sent { get; set; }
        public MessageChannelParticipantListDto Sender { get; set; }

        public MessageListDto(ParticipantMessage participantMessage)
        {
            Text = participantMessage.Message.Text;
            Sent = participantMessage.Message.TimeStamp;
            Sender = new MessageChannelParticipantListDto(participantMessage.Message.Sender);
        }
    }
}
