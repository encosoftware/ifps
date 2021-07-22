using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class AssemblyPlanSeed : IEntitySeed<ManualLaborPlan>
    {
        //public ManualLaborPlan[] Entities => new[]
        //{
        //    new ManualLaborPlan() { Id = 7, WorkStationId = 7, ConcreteFurnitureComponentId = 1, ConcreteFurnitureUnitId = 4, ScheduledStartTime = Clock.Now, ScheduledEndTime = Clock.Now.AddMinutes(5), OptimizationId = Guid.Parse("9505d035-3d31-4938-b072-10347c19e569") },
        //    new ManualLaborPlan() { Id = 8, WorkStationId = 7, ConcreteFurnitureComponentId = 2, ConcreteFurnitureUnitId = 5, ScheduledStartTime = Clock.Now, ScheduledEndTime = Clock.Now.AddMinutes(5), OptimizationId = Guid.Parse("9505d035-3d31-4938-b072-10347c19e569") }
        //};
        public ManualLaborPlan[] Entities => new ManualLaborPlan[] { };
    }
}
