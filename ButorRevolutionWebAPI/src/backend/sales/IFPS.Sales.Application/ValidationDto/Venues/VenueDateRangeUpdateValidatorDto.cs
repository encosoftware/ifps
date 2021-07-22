using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class VenueDateRangeUpdateValidatorDto : AbstractValidator<VenueDateRangeUpdateDto>
    {
        public VenueDateRangeUpdateValidatorDto()
        {
            RuleFor(ent => ent.DayTypeId).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.From).NotNull().NotEmpty();
            RuleFor(ent => ent.To).NotNull().NotEmpty().GreaterThan(e => e.From);
        }
    }
}
