using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class OrderedFurnitureUnitCreateValidatorDto : AbstractValidator<OrderedFurnitureUnitCreateDto>
    {
        public OrderedFurnitureUnitCreateValidatorDto()
        {
            RuleFor(ent => ent.FurnitureUnitId).NotNull().NotEmpty();
            RuleFor(ent => ent.Quantity).NotNull().GreaterThan(0);
        }
    }
}
