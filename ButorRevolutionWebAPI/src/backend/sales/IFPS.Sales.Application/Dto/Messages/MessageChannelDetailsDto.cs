using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class MessageChannelDetailsDto
    {
        public List<MessageListDto> Messages { get; set; }

        public MessageChannelDetailsDto(MessageChannel messageChannel)
        {
            Messages = new List<MessageListDto>();
            foreach (var participant in messageChannel.Participants.Where(participant => participant.ParticipantMessages.Count() > 0))
            {
                Messages.AddRange(participant.ParticipantMessages.Select(ent => new MessageListDto(ent)).ToList());
            }
            Messages = Messages.OrderBy(ent=> ent.Sent).ToList();
        }
    }
}
