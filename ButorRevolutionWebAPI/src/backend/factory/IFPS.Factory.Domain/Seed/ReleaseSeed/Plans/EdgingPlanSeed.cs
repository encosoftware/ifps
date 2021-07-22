using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class EdgingPlanSeed : IEntitySeed<Plan>
    {
        //public Plan[] Entities => new[]
        //{
        //    new Plan() { Id = 5, WorkStationId = 5, ConcreteFurnitureComponentId = 1,  ScheduledStartTime = Clock.Now, ScheduledEndTime = Clock.Now.AddMinutes(5), OptimizationId = Guid.Parse("9505d035-3d31-4938-b072-10347c19e569") },
        //    new Plan() { Id = 6, WorkStationId = 5, ConcreteFurnitureComponentId = 2,  ScheduledStartTime = Clock.Now, ScheduledEndTime = Clock.Now.AddMinutes(5), OptimizationId = Guid.Parse("b65394b2-77d8-4936-bbd4-dda58adf0d71") }
        //};
        public Plan[] Entities => new Plan[] { };
    }
}
