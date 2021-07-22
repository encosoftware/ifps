using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class GroupingCategoryTranslationUpdateDtoValidator : AbstractValidator<GroupingCategoryTranslationUpdateDto>
    {
        public GroupingCategoryTranslationUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);

            RuleFor(x => x.Name).NotNull().NotEmpty();

            RuleFor(x => x.Language).IsInEnum();
        }
    }
}