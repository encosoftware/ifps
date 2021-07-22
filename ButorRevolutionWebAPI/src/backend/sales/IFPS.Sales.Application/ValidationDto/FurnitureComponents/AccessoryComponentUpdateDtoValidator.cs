using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class AccessoryComponentUpdateDtoValidator : AbstractValidator<AccessoryFurnitureUnitUpdateDto>
    {
        public AccessoryComponentUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Amount).NotNull().GreaterThan(0);
            RuleFor(x => x.MaterialId).NotNull().NotEmpty();
            RuleFor(x => x.ImageUpdateDto).SetValidator(new ImageUpdateDtoValidator());
        }
    }
}