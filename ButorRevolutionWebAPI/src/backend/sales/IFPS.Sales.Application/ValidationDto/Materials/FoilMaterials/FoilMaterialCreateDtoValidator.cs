using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class FoilMaterialCreateDtoValidator : AbstractValidator<FoilMaterialCreateDto>
    {
        public FoilMaterialCreateDtoValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.Thickness).NotNull().GreaterThan(0);
            RuleFor(x => x.TransactionMultiplier).NotNull().GreaterThan(0);
            RuleFor(x => x.Price).NotNull().SetValidator(new PriceCreateDtoValidator());
            RuleFor(x => x.ImageCreateDto).NotNull().SetValidator(new ImageCreateDtoValidator());
        }
    }
}