using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class DeliveryPriceCreateValidatorDto : AbstractValidator<DeliveryPriceCreateDto>
    {
        public DeliveryPriceCreateValidatorDto()
        {

        }
    }
}
