using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
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