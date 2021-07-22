using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class AddressUpdateDtoValidator : AbstractValidator<AddressUpdateDto>
    {
        public AddressUpdateDtoValidator()
        {
            RuleFor(x => x.CountryId).NotNull().GreaterThan(0);

            RuleFor(x => x.City).NotNull().NotEmpty();

            RuleFor(x => x.Address).NotNull().NotEmpty();

            RuleFor(x => x.PostCode).NotNull().GreaterThan(0);
        }
    }
}
