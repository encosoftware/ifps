using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class AccessoryComponentCreateDtoValidator : AbstractValidator<AccessoryFurnitureUnitCreateDto>
    {
        public AccessoryComponentCreateDtoValidator()
        {
            RuleFor(x => x.FurnitureUnitId).NotNull();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Amount).NotNull().GreaterThan(0);
            RuleFor(x => x.MaterialId).NotNull().NotEmpty();
        }
    }
}