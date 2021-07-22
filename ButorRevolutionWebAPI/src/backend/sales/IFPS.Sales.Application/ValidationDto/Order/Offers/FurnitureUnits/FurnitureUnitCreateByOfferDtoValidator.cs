using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class FurnitureUnitCreateByOfferDtoValidator : AbstractValidator<FurnitureUnitCreateByOfferDto>
    {
        public FurnitureUnitCreateByOfferDtoValidator()
        {
            RuleFor(ent => ent.FurnitureUnitId).NotNull().NotEmpty();
            RuleFor(ent => ent.Quantity).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
