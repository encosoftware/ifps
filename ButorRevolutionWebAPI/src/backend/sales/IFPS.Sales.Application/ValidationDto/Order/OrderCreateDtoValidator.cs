using FluentValidation;
using IFPS.Sales.Application.Dto;
using System;

namespace IFPS.Sales.Application.ValidationDto.Order
{
    public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
    {
        public OrderCreateDtoValidator()
        {
            RuleFor(x => x.OrderName).NotNull().NotEmpty();
            RuleFor(x => x.CustomerUserId).GreaterThan(0);
            RuleFor(x => x.SalesPersonUserId).GreaterThan(0);
            RuleFor(x => x.Deadline).GreaterThan(DateTime.Now);
            RuleFor(x => x.ShippingAddress).NotNull();
            RuleFor(x => x.ShippingAddress).SetValidator(new AddressCreateDtoValidator());
        }
    }
}
