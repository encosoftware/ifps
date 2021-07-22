using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class CncPlanSeed : IEntitySeed<CncPlan>
    {
        //public CncPlan[] Entities => new[]
        //{
        //    new CncPlan() { Id = 3, WorkStationId = 1, ConcreteFurnitureComponentId = 1,  ScheduledStartTime = Clock.Now, ScheduledEndTime = Clock.Now.AddMinutes(5), OptimizationId = Guid.Parse("9505d035-3d31-4938-b072-10347c19e569"), CncCode = "seed\ncnc\ncode" },
        //    new CncPlan() { Id = 4, WorkStationId = 1, ConcreteFurnitureComponentId = 2,  ScheduledStartTime = Clock.Now, ScheduledEndTime = Clock.Now.AddMinutes(5), OptimizationId = Guid.Parse("b65394b2-77d8-4936-bbd4-dda58adf0d71"), CncCode = "seed\ncnc\ncode" }
        //};
        public CncPlan[] Entities => new CncPlan[] { };
    }
}
