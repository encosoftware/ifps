using FluentValidation;
using IFPS.Factory.Application.Dto;
using System;

namespace IFPS.Factory.Application.Validation
{
    public class InspectionUpdateDtoValidator : AbstractValidator<InspectionUpdateDto>
    {
        public InspectionUpdateDtoValidator()
        {
            RuleFor(x => x.InspectedOn).NotEqual(default(DateTime));
            RuleFor(x => x.ReportName).NotNull().NotEmpty();
            RuleFor(x => x.StorageId).NotNull().GreaterThan(0);
        }
    }
}
