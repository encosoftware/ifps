using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class StorageUpdateDtoValidator : AbstractValidator<StorageUpdateDto>
    {
        public StorageUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Address).SetValidator(new AddressUpdateDtoValidator());
        }
    }
}
