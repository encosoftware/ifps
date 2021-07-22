using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class UpdateOrderedFurnitureUnitQuantityByOfferDtoValidator : AbstractValidator<UpdateOrderedFurnitureUnitQuantityByOfferDto>
    {
        public UpdateOrderedFurnitureUnitQuantityByOfferDtoValidator()
        {
            RuleFor(ent => ent.Quantity).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
