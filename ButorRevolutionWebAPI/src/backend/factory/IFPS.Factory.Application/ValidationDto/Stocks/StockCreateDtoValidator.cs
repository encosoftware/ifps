using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.Validation
{
    public class StockCreateDtoValidator : AbstractValidator<StockCreateDto>
    {
        public StockCreateDtoValidator()
        {
            RuleFor(x => x.PackageId).NotNull().GreaterThan(0);
            RuleFor(x => x.StorageCellId).NotNull().GreaterThan(0);
            RuleFor(x => x.Quantity).NotNull().GreaterThan(0);
        }
    }
}
