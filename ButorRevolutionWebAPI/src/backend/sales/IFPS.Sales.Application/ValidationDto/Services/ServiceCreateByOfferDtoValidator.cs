using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto.Services
{
    public class ServiceCreateByOfferDtoValidator : AbstractValidator<ServiceCreateByOfferDto>
    {
        public ServiceCreateByOfferDtoValidator()
        {
            //RuleForEach(ent => ent.Ids).NotNull().GreaterThan(0);
        }
    }
}
