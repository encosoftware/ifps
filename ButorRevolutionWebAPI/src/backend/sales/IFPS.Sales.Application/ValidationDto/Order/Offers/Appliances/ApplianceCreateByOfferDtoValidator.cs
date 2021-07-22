using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class ApplianceCreateByOfferDtoValidator : AbstractValidator<ApplianceCreateByOfferDto>
    {
        public ApplianceCreateByOfferDtoValidator()
        {
            RuleFor(ent => ent.ApplianceMaterialId).NotNull().NotEmpty();
            RuleFor(ent => ent.Quantity).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
