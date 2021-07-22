using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class GroupingCategoryUpdateDtoValidator : AbstractValidator<GroupingCategoryUpdateDto>
    {
        public GroupingCategoryUpdateDtoValidator()
        {
            RuleForEach(x => x.Translations).SetValidator(x => new GroupingCategoryTranslationUpdateDtoValidator());
        }
    }
}