using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class GroupingCategoryUpdateDtoValidator : AbstractValidator<GroupingCategoryUpdateDto>
    {
        public GroupingCategoryUpdateDtoValidator()
        {
            RuleForEach(x => x.Translations).SetValidator(x => new GroupingCategoryTranslationUpdateDtoValidator());
        }
    }
}