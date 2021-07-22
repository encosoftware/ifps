using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class FoilMaterialUpdateDtoValidator : AbstractValidator<FoilMaterialUpdateDto>
    {
        public FoilMaterialUpdateDtoValidator()
        {
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.Thickness).NotNull().GreaterThan(0);
            RuleFor(x => x.TransactionMultiplier).NotNull().GreaterThan(0);
            RuleFor(x => x.PriceUpdateDto).NotNull().SetValidator(new PriceUpdateDtoValidator());
            RuleFor(x => x.ImageUpdateDto).NotNull().SetValidator(new ImageUpdateDtoValidator());
        }
    }
}