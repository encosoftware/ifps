using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class AccessoryMaterialCreateDtoValidator : AbstractValidator<AccessoryMaterialCreateDto>
    {
        public AccessoryMaterialCreateDtoValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.CategoryId).NotNull().GreaterThan(0);
            RuleFor(x => x.IsOptional).NotNull();
            RuleFor(x => x.IsRequiredForAssembly).NotNull();
            RuleFor(x => x.TransactionMultiplier).NotNull().NotEmpty().GreaterThanOrEqualTo(0);

            RuleFor(x => x.Price).NotNull().SetValidator(new PriceCreateDtoValidator());
            RuleFor(x => x.ImageCreateDto).NotNull().SetValidator(new ImageCreateDtoValidator());
        }
    }
}