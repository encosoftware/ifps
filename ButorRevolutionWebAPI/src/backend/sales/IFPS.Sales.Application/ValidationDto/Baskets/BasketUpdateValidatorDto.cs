using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto.Baskets
{
    public class BasketUpdateValidatorDto : AbstractValidator<BasketUpdateDto>
    {
        public BasketUpdateValidatorDto()
        {
            RuleFor(ent => ent.DeliveryPrice).SetValidator(new PriceCreateDtoValidator());
            RuleForEach(ent => ent.OrderedFurnitureUnits).SetValidator(new OrderedFurnitureUnitUpdateValidatorDto());
        }
    }
}
