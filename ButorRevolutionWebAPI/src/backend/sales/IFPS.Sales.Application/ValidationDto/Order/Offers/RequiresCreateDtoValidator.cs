using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class RequiresCreateDtoValidator : AbstractValidator<RequiresCreateDto>
    {
        public RequiresCreateDtoValidator()
        {
            RuleFor(ent => ent.Budget).SetValidator(new PriceCreateDtoValidator());
            RuleFor(ent => ent.IsPrivatePerson).NotNull();
        }
    }
}
