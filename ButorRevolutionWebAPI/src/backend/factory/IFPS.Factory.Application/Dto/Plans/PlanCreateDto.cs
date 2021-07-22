using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class PlanCreateDto
    {
        public List<ManualLabourPlanByTypeCreateDto> AssemblyPlans { get; set; }
        public List<CncPlanByTypeCreateDto> CncPlans { get; set; }
        public List<PlanByTypeCreateDto> EdgingPlans { get; set; }
        public List<LayoutPlanCreateDto> LayoutPlans { get; set; }
        public List<ManualLabourPlanByTypeCreateDto> PackingPlans { get; set; }
        public List<ManualLabourPlanByTypeCreateDto> SortingPlans { get; set; }

        public PlanCreateDto()
        {
            
        }

        public Dictionary<Plan, Guid> SetProperties()
        {
            var planOrderIdDictionary = new Dictionary<Plan, Guid>();

            foreach (var assemblyPlan in AssemblyPlans)
            {
                var assembly = assemblyPlan.CreateModelObject();
                planOrderIdDictionary.Add(assembly, assemblyPlan.OrderId.Value);
            }

            foreach (var cncPlan in CncPlans)
            {
                var cnc = cncPlan.CreateModelObject();
                planOrderIdDictionary.Add(cnc, cncPlan.OrderId.Value);
            }

            foreach (var edgingPlan in EdgingPlans)
            {
                var edging = edgingPlan.CreateModelObject();
                planOrderIdDictionary.Add(edging, edgingPlan.OrderId.Value);
            }

            foreach (var layout in LayoutPlans)
            {
                var plan = layout.CreateModelObject();

                foreach (var cutting in layout.Cuttings)
                {
                    plan.AddCutting(cutting.CreateModelObject(plan.Id));
                }
                planOrderIdDictionary.Add(plan, layout.OrderIds.First());
            }

            foreach (var packingPlan in PackingPlans)
            {
                var packing = packingPlan.CreateModelObject();
                planOrderIdDictionary.Add(packing, packingPlan.OrderId.Value);
            }

            foreach (var sortingPlan in SortingPlans)
            {
                var sorting = sortingPlan.CreateModelObject();
                planOrderIdDictionary.Add(sorting, sortingPlan.OrderId.Value);
            }

            return planOrderIdDictionary;
        }
    }
}
