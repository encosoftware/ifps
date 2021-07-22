using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto.Order.Offers
{
    public class OfferCreateDtoValidator : AbstractValidator<OfferCreateDto>
    {
        public OfferCreateDtoValidator()
        {
            RuleFor(ent => ent.Requires).SetValidator(new RequiresCreateDtoValidator());
            RuleFor(ent => ent.BaseCabinet).SetValidator(new CabinetMaterialCreateDtoValidator());
            RuleFor(ent => ent.TopCabinet).SetValidator(new CabinetMaterialCreateDtoValidator());
        }
    }
}
