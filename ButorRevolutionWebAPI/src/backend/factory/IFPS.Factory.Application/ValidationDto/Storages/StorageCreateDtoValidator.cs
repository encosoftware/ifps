using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class StorageCreateDtoValidator : AbstractValidator<StorageCreateDto>
    {
        public StorageCreateDtoValidator()
        {
            RuleFor(x => x.CompanyId).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Address).SetValidator(new AddressCreateDtoValidator());
        }
    }
}
