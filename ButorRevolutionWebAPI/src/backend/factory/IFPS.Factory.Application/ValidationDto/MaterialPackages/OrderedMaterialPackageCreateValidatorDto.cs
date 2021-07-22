using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class OrderedMaterialPackageCreateValidatorDto : AbstractValidator<OrderedMaterialPackageCreateDto>
    {
        public OrderedMaterialPackageCreateValidatorDto()
        {
            RuleFor(ent => ent.OrderedPackageNum).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ent => ent.PackageId).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
