using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
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
