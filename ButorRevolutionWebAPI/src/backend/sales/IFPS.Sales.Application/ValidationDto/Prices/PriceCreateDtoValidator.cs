using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class PriceCreateDtoValidator : AbstractValidator<PriceCreateDto>
    {
        public PriceCreateDtoValidator()
        {
            RuleFor(x => x.CurrencyId).NotNull().GreaterThan(0);
            RuleFor(x => x.Value).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}