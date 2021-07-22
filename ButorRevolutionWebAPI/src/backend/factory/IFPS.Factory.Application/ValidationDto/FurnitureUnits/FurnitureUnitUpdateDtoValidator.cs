using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class FurnitureUnitUpdateDtoValidator : AbstractValidator<FurnitureUnitUpdateDto>
    {
        public FurnitureUnitUpdateDtoValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.CategoryId).NotNull().GreaterThan(0);
            RuleFor(x => x.Width).NotNull().GreaterThan(0);
            RuleFor(x => x.Height).NotNull().GreaterThan(0);
            RuleFor(x => x.Depth).NotNull().GreaterThan(0);
            RuleFor(x => x.ImageUpdateDto).SetValidator(new ImageUpdateDtoValidator());
        }
    }
}