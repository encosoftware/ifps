using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class CompanyDateRangeUpdateDtoValidator : AbstractValidator<CompanyDateRangeUpdateDto>
    {
        public CompanyDateRangeUpdateDtoValidator()
        {
            RuleFor(x => x.DayTypeId).NotNull().GreaterThan(0);
            RuleFor(x => x.From).NotNull().NotEmpty();
            RuleFor(x => x.To).NotNull().NotEmpty().GreaterThan(x => x.From);
        }
    }
}