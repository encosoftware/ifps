using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class DimensionCreateDtoValidator : AbstractValidator<DimensionCreateDto>
    {
        public DimensionCreateDtoValidator()
        {
            RuleFor(x => x.Length).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Thickness).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Width).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}