using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class AssemblyPlanTestSeed : IEntitySeed<ManualLaborPlan>
    {
        public ManualLaborPlan[] Entities => new[]
        {
            new ManualLaborPlan() { Id = 10006,  ScheduledStartTime = new DateTime(2019, 8, 5), ScheduledEndTime = new DateTime(2019, 8, 5).AddMinutes(5), ConcreteFurnitureUnitId = 10000, OptimizationId = Guid.Parse("3908bed6-4469-4bdf-96b9-b477e9a96479") },
            new ManualLaborPlan() { Id = 10007,  ScheduledStartTime = new DateTime(2019, 8, 5), ScheduledEndTime = new DateTime(2019, 8, 5).AddMinutes(5), ConcreteFurnitureUnitId = 10001, OptimizationId = Guid.Parse("3908bed6-4469-4bdf-96b9-b477e9a96479") },

            new ManualLaborPlan() 
            { 
                Id = 10009, 
                ScheduledStartTime = new DateTime(2020, 2, 4, 8, 0, 0), 
                ScheduledEndTime = new DateTime(2020, 2, 4, 12, 0, 0), 
                OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"), 
                ConcreteFurnitureUnitId = 10002,
                WorkStationId = 10008
            },
            new ManualLaborPlan() 
            { 
                Id = 10010, 
                ScheduledStartTime = new DateTime(2020, 3, 13, 9, 15, 0), 
                ScheduledEndTime = new DateTime(2020, 3, 13, 11, 0, 0), 
                OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"), 
                ConcreteFurnitureUnitId = 10002,
                WorkStationId = 10008
            }
        };
    }
}
