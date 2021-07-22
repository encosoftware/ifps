using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class WorkingHourDtoValidator : AbstractValidator<WorkingHourDto>
    {
        public WorkingHourDtoValidator()
        {
            RuleFor(x => x.DayTypeId).NotNull();
            RuleFor(x => x.From).NotNull();
            RuleFor(x => x.To).NotNull();
            RuleFor(x => x.From.TimeOfDay).LessThan(x => x.To.TimeOfDay);
        }
    }
}