using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class PlanCreateValidatorDto : AbstractValidator<PlanCreateDto>
    {
        public PlanCreateValidatorDto()
        {
            RuleForEach(ent => ent.AssemblyPlans).SetValidator(new ManualLabourPlanByTypeCreateValidatorDto());
            RuleForEach(ent => ent.CncPlans).SetValidator(new CncPlanByTypeCreateValidatorDto());
            RuleForEach(ent => ent.EdgingPlans).SetValidator(new PlanByTypeCreateValidatorDto());
            RuleForEach(ent => ent.PackingPlans).SetValidator(new ManualLabourPlanByTypeCreateValidatorDto());
            RuleForEach(ent => ent.SortingPlans).SetValidator(new ManualLabourPlanByTypeCreateValidatorDto());
            RuleForEach(ent => ent.LayoutPlans).SetValidator(new LayoutPlanCreateValidatorDto());
        }
    }
}