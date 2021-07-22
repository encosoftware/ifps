using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class CncPlanByTypeCreateValidatorDto : AbstractValidator<CncPlanByTypeCreateDto>
    {
        public CncPlanByTypeCreateValidatorDto()
        {
            RuleFor(ent => ent.WorkStationId).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.ScheduledStartHour).NotNull().NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(ent => ent.ScheduledStartMinute).NotNull().NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(ent => ent.ScheduledEndHour).NotNull().NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(ent => ent.ScheduledEndMinute).NotNull().NotEmpty().GreaterThanOrEqualTo(0);
        }
    }
}
