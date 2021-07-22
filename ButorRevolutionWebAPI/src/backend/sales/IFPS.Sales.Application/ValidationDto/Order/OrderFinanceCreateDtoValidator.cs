using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class OrderFinanceCreateDtoValidator : AbstractValidator<OrderFinanceCreateDto>
    {
        public OrderFinanceCreateDtoValidator()
        {
            RuleFor(ent => ent.PaymentDate).NotNull().NotEmpty();
            RuleFor(ent => ent.PaymentIndex).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
