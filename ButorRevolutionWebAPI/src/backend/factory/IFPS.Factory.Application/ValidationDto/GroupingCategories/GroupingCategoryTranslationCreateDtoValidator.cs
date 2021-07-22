using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    internal class GroupingCategoryTranslationCreateDtoValidator : AbstractValidator<GroupingCategoryTranslationCreateDto>
    {
        public GroupingCategoryTranslationCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();

            RuleFor(x => x.Language).IsInEnum();
        }
    }
}