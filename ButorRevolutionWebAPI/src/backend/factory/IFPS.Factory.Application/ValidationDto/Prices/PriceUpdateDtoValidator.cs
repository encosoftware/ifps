using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class PriceUpdateDtoValidator : AbstractValidator<PriceUpdateDto>
    {
        public PriceUpdateDtoValidator()
        {
            RuleFor(x => x.CurrencyId).NotNull().GreaterThan(0);
            RuleFor(x => x.Value).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
