using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class ContractCreateDtoValidator : AbstractValidator<ContractCreateDto>
    {
        public ContractCreateDtoValidator()
        {
            RuleFor(ent => ent.Additional).NotNull().NotEmpty();
            RuleFor(ent => ent.ContractDate).NotNull().NotEmpty();
            RuleFor(ent => ent.FirstPayment).SetValidator(new PriceCreateDtoValidator());
            RuleFor(ent => ent.FirstPaymentDate).NotNull().NotEmpty();
            RuleFor(ent => ent.SecondPayment).SetValidator(new PriceCreateDtoValidator());
            RuleFor(ent => ent.SecondPaymentDate).NotNull().NotEmpty();
        }
    }
}
