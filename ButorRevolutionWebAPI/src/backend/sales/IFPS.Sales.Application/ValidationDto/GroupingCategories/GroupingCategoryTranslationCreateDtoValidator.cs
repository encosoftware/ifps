using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
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