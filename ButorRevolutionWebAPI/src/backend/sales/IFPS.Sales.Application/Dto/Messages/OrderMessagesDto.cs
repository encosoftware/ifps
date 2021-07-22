using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class OrderMessagesDto
    {
        public List<UnansweredMessageListDto> MessageChannels { get; set; }
        public List<MessageChannelParticipantListDto> Participants { get; set; }

        public OrderMessagesDto()
        {
            MessageChannels = new List<UnansweredMessageListDto>();
            Participants = new List<MessageChannelParticipantListDto>();
        }
    }
}
