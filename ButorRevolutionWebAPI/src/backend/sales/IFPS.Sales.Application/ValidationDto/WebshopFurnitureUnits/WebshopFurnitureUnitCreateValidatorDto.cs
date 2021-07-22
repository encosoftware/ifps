using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class WebshopFurnitureUnitCreateValidatorDto : AbstractValidator<WebshopFurnitureUnitCreateDto>
    {
        public WebshopFurnitureUnitCreateValidatorDto()
        {
            RuleFor(ent => ent.FurnitureUnitId).NotNull().NotEmpty();
            RuleForEach(ent => ent.Images).SetValidator(new ImageCreateDtoValidator());
            RuleFor(ent => ent.Price).SetValidator(new PriceCreateDtoValidator());
        }
    }
}
