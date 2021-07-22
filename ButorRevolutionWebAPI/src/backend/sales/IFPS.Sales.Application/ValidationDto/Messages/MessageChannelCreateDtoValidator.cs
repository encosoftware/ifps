using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class MessageChannelCreateDtoValidator : AbstractValidator<MessageChannelCreateDto>
    {
        public MessageChannelCreateDtoValidator()
        {
            RuleFor(ent => ent.OrderId).NotNull().NotEmpty();
            RuleForEach(ent => ent.ParticipantIds).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
