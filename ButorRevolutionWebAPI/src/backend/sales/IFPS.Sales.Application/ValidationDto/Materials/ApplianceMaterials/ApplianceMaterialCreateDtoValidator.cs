using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class ApplianceMaterialCreateDtoValidator : AbstractValidator<ApplianceMaterialCreateDto>
    {
        public ApplianceMaterialCreateDtoValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.CategoryId).NotNull().GreaterThan(0);
            RuleFor(x => x.CompanyId).NotNull().GreaterThan(0);
            RuleFor(x => x.HanaCode).NotNull().NotEmpty();
            RuleFor(x => x.PurchasingPrice).NotNull().SetValidator(new PriceCreateDtoValidator());
            RuleFor(x => x.SellPrice).NotNull().SetValidator(new PriceCreateDtoValidator());
            RuleFor(x => x.ImageCreateDto).NotNull().SetValidator(new ImageCreateDtoValidator());
        }
    }
}