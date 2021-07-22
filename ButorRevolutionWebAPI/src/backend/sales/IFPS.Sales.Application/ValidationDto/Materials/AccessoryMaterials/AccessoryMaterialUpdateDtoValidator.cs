using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class AccessoryMaterialUpdateDtoValidator : AbstractValidator<AccessoryMaterialUpdateDto>
    {
        public AccessoryMaterialUpdateDtoValidator()
        {
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.CategoryId).NotNull().GreaterThan(0);
            RuleFor(x => x.TransactionMultiplier).NotNull().NotEmpty().GreaterThanOrEqualTo(0);

            RuleFor(x => x.PriceUpdateDto).NotNull().SetValidator(new PriceUpdateDtoValidator());
            RuleFor(x => x.ImageUpdateDto).NotNull().SetValidator(new ImageUpdateDtoValidator());
        }
    }
}