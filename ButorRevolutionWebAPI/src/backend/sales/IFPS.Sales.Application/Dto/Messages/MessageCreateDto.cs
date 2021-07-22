using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class MessageCreateDto
    {
        public string Text { get; set; }
        public int SenderId { get; set; }
        public int MessageChannelId { get; set; }

        public Message CreateModelObject()
        {
            return new Message(MessageChannelId, SenderId, Text);
        }
    }
}
