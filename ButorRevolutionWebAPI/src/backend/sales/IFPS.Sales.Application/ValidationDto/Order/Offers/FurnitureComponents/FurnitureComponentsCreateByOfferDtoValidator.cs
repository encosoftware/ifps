using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class FurnitureComponentsCreateByOfferDtoValidator : AbstractValidator<FurnitureComponentsCreateByOfferDto>
    {
        public FurnitureComponentsCreateByOfferDtoValidator()
        {
            RuleFor(ent => ent.Id).NotNull().NotEmpty();
            RuleFor(ent => ent.Name).NotNull().NotEmpty();
            RuleFor(ent => ent.Width).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.Height).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.Amount).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
