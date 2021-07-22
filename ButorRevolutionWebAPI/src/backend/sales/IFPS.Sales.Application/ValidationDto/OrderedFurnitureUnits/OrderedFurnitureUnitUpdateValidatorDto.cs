using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class OrderedFurnitureUnitUpdateValidatorDto : AbstractValidator<OrderedFurnitureUnitUpdateDto>
    {
        public OrderedFurnitureUnitUpdateValidatorDto()
        {
            RuleFor(ent => ent.FurnitureUnitId).NotNull().NotEmpty();
            RuleFor(ent => ent.Quantity).NotNull().GreaterThan(0);
        }
    }
}
