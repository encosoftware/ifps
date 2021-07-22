using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class DecorBoardMaterialCreateDtoValidator : AbstractValidator<DecorBoardMaterialCreateDto>
    {
        public DecorBoardMaterialCreateDtoValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.CategoryId).NotNull().GreaterThan(0);
            RuleFor(x => x.TransactionMultiplier).NotNull().GreaterThan(0);
            RuleFor(x => x.HasFiberDirection).NotNull();

            RuleFor(x => x.Price).NotNull().SetValidator(new PriceCreateDtoValidator());
            RuleFor(x => x.ImageCreateDto).NotNull().SetValidator(new ImageCreateDtoValidator());
            RuleFor(x => x.Dimension).NotNull().SetValidator(new DimensionCreateDtoValidator());
        }
    }
}