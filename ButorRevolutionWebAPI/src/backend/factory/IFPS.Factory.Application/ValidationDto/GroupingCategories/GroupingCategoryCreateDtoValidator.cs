using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class GroupingCategoryCreateDtoValidator : AbstractValidator<GroupingCategoryCreateDto>
    {
        public GroupingCategoryCreateDtoValidator()
        {
            RuleForEach(x => x.Translations).SetValidator(x => new GroupingCategoryTranslationCreateDtoValidator());
        }
    }
}