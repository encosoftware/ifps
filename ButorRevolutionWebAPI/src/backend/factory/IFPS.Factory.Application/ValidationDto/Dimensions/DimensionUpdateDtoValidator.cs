using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class DimensionUpdateDtoValidator : AbstractValidator<DimensionUpdateDto>
    {
        public DimensionUpdateDtoValidator()
        {
            RuleFor(x => x.Length).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Thickness).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Width).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
