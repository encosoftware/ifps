using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto.WebshopFurnitureUnits
{
    public class WebshopFurnitureUnitUpdateValidatorDto : AbstractValidator<WebshopFurnitureUnitUpdateDto>
    {
        public WebshopFurnitureUnitUpdateValidatorDto()
        {
            RuleFor(ent => ent.Price).SetValidator(new PriceCreateDtoValidator());
            RuleForEach(ent => ent.Images).SetValidator(new ImageCreateDtoValidator());
        }
    }
}
