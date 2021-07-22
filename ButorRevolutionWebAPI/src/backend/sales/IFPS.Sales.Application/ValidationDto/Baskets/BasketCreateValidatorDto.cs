using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class BasketCreateValidatorDto : AbstractValidator<BasketCreateDto>
    {
        public BasketCreateValidatorDto()
        {
            RuleForEach(ent => ent.OrderedFurnitureUnits).SetValidator(new OrderedFurnitureUnitCreateValidatorDto());
            RuleFor(ent => ent.DeliveryPrice).SetValidator(new DeliveryPriceCreateValidatorDto());
        }
    }
}
