using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class PackingPlanTestSeed : IEntitySeed<ManualLaborPlan>
    {
        public ManualLaborPlan[] Entities => new[]
        {
            new ManualLaborPlan() 
            { 
                Id = 10011, 
                OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                ScheduledStartTime = new DateTime(2020, 2, 4, 13, 0, 0),
                ScheduledEndTime = new DateTime(2020, 2, 4, 17, 0, 0),
                ConcreteFurnitureUnitId = 10002,
                WorkStationId = 10009
            },
            new ManualLaborPlan()
            { 
                Id = 10012, 
                OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                ScheduledStartTime = new DateTime(2020, 3, 13, 13, 20, 0),
                ScheduledEndTime = new DateTime(2020, 3, 13, 16, 45, 0),
                ConcreteFurnitureUnitId = 10002,
                WorkStationId = 10009
            }
        };
    }
}
