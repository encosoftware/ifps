using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.Validation
{
    public class StorageCellCreateDtoValidator : AbstractValidator<StorageCellCreateDto>
    {
        public StorageCellCreateDtoValidator()
        {
            RuleFor(x => x.StorageId).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
        }
    }
}
