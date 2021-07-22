using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class ApplianceMaterialUpdateDtoValidator : AbstractValidator<ApplianceMaterialUpdateDto>
    {
        public ApplianceMaterialUpdateDtoValidator()
        {
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.CategoryId).NotNull().GreaterThan(0);
            RuleFor(x => x.CompanyId).NotNull().GreaterThan(0);
            RuleFor(x => x.HanaCode).NotNull().NotEmpty();
            RuleFor(x => x.PurchasingPrice).NotNull().SetValidator(new PriceUpdateDtoValidator());
            RuleFor(x => x.SellPrice).NotNull().SetValidator(new PriceUpdateDtoValidator());
            RuleFor(x => x.ImageUpdateDto).NotNull().SetValidator(new ImageUpdateDtoValidator());
        }
    }
}