using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class CargoCreateValidatorDto : AbstractValidator<CargoCreateDto>
    {
        public CargoCreateValidatorDto()
        {
            RuleFor(ent => ent.BookedById).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.CargoName).NotNull().NotEmpty();
            RuleFor(ent => ent.ShippingAddress).SetValidator(new AddressCreateDtoValidator());
            RuleFor(ent => ent.ShippingCost).SetValidator(new PriceCreateDtoValidator());
            RuleFor(ent => ent.SupplierId).NotNull().NotEmpty().GreaterThan(0);

            RuleForEach(ent => ent.Additionals).SetValidator(new OrderedMaterialPackageCreateValidatorDto());
            RuleForEach(ent => ent.Materials).SetValidator(new OrderedMaterialPackageCreateValidatorDto());
        }
    }
}
