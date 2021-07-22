using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class VenueCreateDtoValidator : AbstractValidator<VenueCreateDto>
    {
        public VenueCreateDtoValidator()
        {
            RuleFor(r => r.Name).NotNull().NotEmpty();

            RuleFor(r => r.PhoneNumber).NotNull().NotEmpty();

            RuleFor(r => r.Email).NotNull().NotEmpty();

            RuleFor(r => r.OfficeAddress).SetValidator(new AddressCreateDtoValidator());
        }
    }
}