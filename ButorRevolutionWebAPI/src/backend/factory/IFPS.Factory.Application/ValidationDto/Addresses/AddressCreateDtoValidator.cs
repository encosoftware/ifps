using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class AddressCreateDtoValidator : AbstractValidator<AddressCreateDto>
    {
        public AddressCreateDtoValidator()
        {
            RuleFor(x => x.CountryId).NotNull().GreaterThan(0);

            RuleFor(x => x.City).NotNull().NotEmpty();

            RuleFor(x => x.Address).NotNull().NotEmpty();

            RuleFor(x => x.PostCode).NotNull().GreaterThan(0);
        }
    }
}
