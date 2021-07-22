using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class CuttingCreateValidatorDto : AbstractValidator<CuttingCreateDto>
    {
        public CuttingCreateValidatorDto()
        {
            RuleFor(ent => ent.Height).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.Width).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.TopLeftX).NotNull().NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(ent => ent.TopLeftY).NotNull().NotEmpty().GreaterThanOrEqualTo(0);
        }
    }
}
