using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.Validation
{
    public class StockReserveDtoValidator : AbstractValidator<StockReserveDto>
    {
        public StockReserveDtoValidator()
        {
            RuleFor(x => x.OrderId).NotNull();
            RuleForEach(x => x.StockQuantities).SetValidator(new StockQuantityDtoValidator());
        }
    }
}
