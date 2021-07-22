using FluentValidation;
using IFPS.Sales.Application.Dto;
using System;

namespace IFPS.Sales.Application.ValidationDto.Order
{
    public class OrderEditDtoValidator : AbstractValidator<OrderEditDto>
    {
        public OrderEditDtoValidator()
        {
            RuleFor(x => x.AssignedToUserId).Must(x => !x.HasValue || (x.HasValue && x.Value>0) );
            RuleFor(x => x.CurrentStatusId).GreaterThan(0);
            RuleFor(x => x.CustomerUserId).GreaterThan(0);
            RuleFor(x => x.Deadline).GreaterThan(DateTime.Now);
            RuleFor(x => x.SalesPersonUserId).GreaterThan(0);
            RuleFor(x => x.ShippingAddress).NotNull();
            RuleFor(x => x.ShippingAddress).SetValidator( new AddressCreateDtoValidator());
            RuleFor(x => x.StatusDeadline).GreaterThan(DateTime.Now).LessThanOrEqualTo(x => x.Deadline);
        }
    }
}
