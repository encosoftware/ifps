using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class CabinetMaterialCreateDtoValidator : AbstractValidator<CabinetMaterialCreateDto>
    {
        public CabinetMaterialCreateDtoValidator()
        {
            RuleFor(ent => ent.Height).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.Width).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.DoorMaterialId).NotNull().NotEmpty();
            RuleFor(ent => ent.InnerMaterialId).NotNull().NotEmpty();
            RuleFor(ent => ent.OuterMaterialId).NotNull().NotEmpty();
            RuleFor(ent => ent.BackPanelMaterialId).NotNull().NotEmpty();
        }
    }
}
