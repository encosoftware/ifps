using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class SortingPlanTestSeed : IEntitySeed<ManualLaborPlan>
    {
        public ManualLaborPlan[] Entities => new[]
        {
            new ManualLaborPlan() 
            { 
                Id = 10013, 
                OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                ScheduledStartTime = new DateTime(2020, 2, 5, 8, 0, 0),
                ScheduledEndTime = new DateTime(2020, 2, 5, 10, 0, 0),
                ConcreteFurnitureUnitId = 10002,
                WorkStationId = 10010
            },
            new ManualLaborPlan() 
            { 
                Id = 10014, 
                OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                ScheduledStartTime = new DateTime(2020, 3, 14, 8, 30, 0),
                ScheduledEndTime = new DateTime(2020, 3, 14, 11, 0, 0),
                ConcreteFurnitureUnitId = 10002,
                WorkStationId = 10010
            }
        };
    }
}
