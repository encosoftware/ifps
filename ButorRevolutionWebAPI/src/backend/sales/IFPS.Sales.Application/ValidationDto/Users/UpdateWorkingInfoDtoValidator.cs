using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class UpdateWorkingInfoDtoValidator : AbstractValidator<UpdateWorkingInfoDto>
    {
        public UpdateWorkingInfoDtoValidator()
        {
            RuleFor(x => x.MinDiscountPercent).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(100).LessThanOrEqualTo(x => x.MaxDiscountPercent);
            RuleFor(x => x.MaxDiscountPercent).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);

            RuleForEach(x => x.WorkingHours).SetValidator(new WorkingHourDtoValidator());
        }
    }
}