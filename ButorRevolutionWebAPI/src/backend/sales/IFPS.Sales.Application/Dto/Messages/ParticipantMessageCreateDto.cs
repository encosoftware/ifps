using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class ParticipantMessageCreateDto
    {
        public int RecipientId { get; set; }
        public MessageCreateDto Message { get; set; }
    }
}
