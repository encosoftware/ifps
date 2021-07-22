using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class CompanyCreateDtoValidator : AbstractValidator<CompanyCreateDto>
    {
        public CompanyCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();

            RuleFor(x => x.TaxNumber).NotNull().NotEmpty();

            RuleFor(x => x.RegisterNumber).NotNull().NotEmpty();

            RuleFor(x => x.CompanyTypeId).NotNull().GreaterThan(0);

            RuleFor(x => x.Address).SetValidator(new AddressCreateDtoValidator());
        }
    }
}