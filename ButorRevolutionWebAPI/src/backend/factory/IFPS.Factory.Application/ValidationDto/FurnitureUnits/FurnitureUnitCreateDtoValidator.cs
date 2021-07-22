using IFPS.Factory.Application.Dto;
using FluentValidation;

namespace IFPS.Factory.Application.ValidationDto
{
    public class FurnitureUnitCreateDtoValidator : AbstractValidator<FurnitureUnitCreateDto>
    {
        public FurnitureUnitCreateDtoValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.CategoryId).NotNull().GreaterThan(0);
            RuleFor(x => x.Width).NotNull().GreaterThan(0);
            RuleFor(x => x.Height).NotNull().GreaterThan(0);
            RuleFor(x => x.Depth).NotNull().GreaterThan(0);
            RuleFor(x => x.ImageCreateDto).SetValidator(new ImageCreateDtoValidator());
        }
    }
}
