using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class DecorBoardMaterialUpdateDtoValidator : AbstractValidator<DecorBoardMaterialUpdateDto>
    {
        public DecorBoardMaterialUpdateDtoValidator()
        {
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.CategoryId).NotNull().GreaterThan(0);
            RuleFor(x => x.TransactionMultiplier).NotNull().GreaterThan(0);
            RuleFor(x => x.HasFiberDirection).NotNull();
            RuleFor(x => x.PriceUpdateDto).NotNull().SetValidator(new PriceUpdateDtoValidator());
            RuleFor(x => x.ImageUpdateDto).NotNull().SetValidator(new ImageUpdateDtoValidator());
            RuleFor(x => x.Dimension).NotNull().SetValidator(new DimensionUpdateDtoValidator());
        }
    }
}