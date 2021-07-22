using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class FurnitureComponentsCreateWithoutIdByOfferDtoValidator : AbstractValidator<FurnitureComponentsCreateWithoutIdByOfferDto>
    {
        public FurnitureComponentsCreateWithoutIdByOfferDtoValidator()
        {
            RuleFor(ent => ent.Name).NotNull().NotEmpty();
            RuleFor(ent => ent.Width).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.Height).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.Amount).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.FurnitureUnitId).NotNull().NotEmpty();
        }
    }
}
