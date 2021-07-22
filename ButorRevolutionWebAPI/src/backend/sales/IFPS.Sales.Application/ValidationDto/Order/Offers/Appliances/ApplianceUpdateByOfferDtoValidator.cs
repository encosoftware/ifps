using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class ApplianceUpdateByOfferDtoValidator : AbstractValidator<ApplianceUpdateByOfferDto>
    {
        public ApplianceUpdateByOfferDtoValidator()
        {
            RuleFor(ent => ent.Quantity).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
