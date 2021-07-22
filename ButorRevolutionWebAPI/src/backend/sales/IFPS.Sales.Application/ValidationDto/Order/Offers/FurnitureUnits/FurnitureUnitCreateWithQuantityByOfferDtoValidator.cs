using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class FurnitureUnitCreateWithQuantityByOfferDtoValidator : AbstractValidator<FurnitureUnitCreateWithQuantityByOfferDto>
    {
        public FurnitureUnitCreateWithQuantityByOfferDtoValidator()
        {
            RuleFor(ent => ent.CategoryId).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.Code).NotNull().NotEmpty();
            RuleFor(ent => ent.Depth).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.Height).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.Width).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.Quantity).NotNull().NotEmpty().GreaterThan(0);

            RuleForEach(ent => ent.Corpuses).SetValidator(new FurnitureComponentsCreateByOfferDtoValidator());
            RuleForEach(ent => ent.Fronts).SetValidator(new FurnitureComponentsCreateByOfferDtoValidator());
        }
    }
}
