using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class MessageCreateDtoValidator : AbstractValidator<MessageCreateDto>
    {
        public MessageCreateDtoValidator()
        {
            RuleFor(ent => ent.Text).NotNull().NotEmpty();
            RuleFor(ent => ent.SenderId).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.MessageChannelId).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
