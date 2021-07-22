using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class CompanyUpdateDtoValidator : AbstractValidator<CompanyUpdateDto>
    {
        public CompanyUpdateDtoValidator()
        {
            RuleFor(x => x.TaxNumber).NotNull().NotEmpty();

            RuleFor(x => x.RegisterNumber).NotNull().NotEmpty();

            RuleFor(x => x.CompanyTypeId).NotNull().GreaterThan(0);

            RuleFor(x => x.Address).SetValidator(new AddressCreateDtoValidator());
            RuleForEach(x => x.UserTeams).SetValidator(new UserTeamUpdateDtoValidator());
            RuleForEach(x => x.OpeningHours).SetValidator(new CompanyDateRangeUpdateDtoValidator());
        }
    }
}