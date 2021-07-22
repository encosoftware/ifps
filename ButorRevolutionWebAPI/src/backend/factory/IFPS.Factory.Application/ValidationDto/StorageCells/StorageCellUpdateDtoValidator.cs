using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.Validation
{
    public class StorageCellUpdateDtoValidator : AbstractValidator<StorageCellUpdateDto>
    {
        public StorageCellUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
        }
    }
}
