using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class GroupingCategoryCreateDtoValidator : AbstractValidator<GroupingCategoryCreateDto>
    {
        public GroupingCategoryCreateDtoValidator()
        {
            RuleForEach(x => x.Translations).SetValidator(x => new GroupingCategoryTranslationCreateDtoValidator());
        }
    }
}