using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.Validation
{
    public class StockQuantityDtoValidator : AbstractValidator<StockQuantityDto>
    {
        public StockQuantityDtoValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Quantity).NotNull().GreaterThan(0);
        }
    }
}
