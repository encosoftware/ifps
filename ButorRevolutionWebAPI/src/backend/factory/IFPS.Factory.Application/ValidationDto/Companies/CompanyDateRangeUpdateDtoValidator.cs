using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class CompanyDateRangeUpdateDtoValidator : AbstractValidator<CompanyDateRangeUpdateDto>
    {
        public CompanyDateRangeUpdateDtoValidator()
        {
            RuleFor(x => x.DayTypeId).NotNull().GreaterThan(0);
            RuleFor(x => x.From).NotNull().NotEmpty();
            RuleFor(x => x.To).NotNull().NotEmpty().GreaterThan(x=>x.From);
        }
    }
}
