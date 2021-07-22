using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class PackingPlanSeed : IEntitySeed<ManualLaborPlan>
    {
        //public ManualLaborPlan[] Entities => new[]
        //{
        //    new ManualLaborPlan() { Id = 10, WorkStationId = 8, ConcreteFurnitureComponentId = 1, ScheduledStartTime = Clock.Now, ScheduledEndTime = Clock.Now.AddMinutes(5), OptimizationId = Guid.Parse("9505d035-3d31-4938-b072-10347c19e569") }
        //};
        public ManualLaborPlan[] Entities => new ManualLaborPlan[] { };
    }
}
