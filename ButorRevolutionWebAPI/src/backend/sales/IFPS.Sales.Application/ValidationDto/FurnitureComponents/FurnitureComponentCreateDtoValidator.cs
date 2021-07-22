using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class FurnitureComponentCreateDtoValidator : AbstractValidator<FurnitureComponentCreateDto>
    {
        public FurnitureComponentCreateDtoValidator()
        {
            RuleFor(x => x.FurnitureUnitId).NotNull();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Width).NotNull().GreaterThan(0);
            RuleFor(x => x.Height).NotNull().GreaterThan(0);
            RuleFor(x => x.Type).NotNull().NotEmpty();
            RuleFor(x => x.Amount).NotNull().GreaterThan(0);
            RuleFor(x => x.MaterialId).NotNull().NotEmpty();
            RuleFor(x => x.TopFoilId).NotNull().NotEmpty();
            RuleFor(x => x.BottomFoilId).NotNull().NotEmpty();
            RuleFor(x => x.LeftFoilId).NotNull().NotEmpty();
            RuleFor(x => x.RightFoilId).NotNull().NotEmpty();
        }
    }
}