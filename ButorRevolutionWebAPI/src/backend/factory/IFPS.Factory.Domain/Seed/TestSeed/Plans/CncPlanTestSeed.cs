using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class CncPlanTestSeed : IEntitySeed<CncPlan>
    {
        public CncPlan[] Entities => new[]
        {
            new CncPlan() { Id = 10002, WorkStationId = 10000, ConcreteFurnitureComponentId = 10000, ScheduledStartTime = new DateTime(2019, 8, 5), ScheduledEndTime = new DateTime(2019, 8, 5).AddMinutes(5), OptimizationId = Guid.Parse("3908bed6-4469-4bdf-96b9-b477e9a96479") },
            new CncPlan() { Id = 10003, WorkStationId = 10000, ConcreteFurnitureComponentId = 10001, ScheduledStartTime = new DateTime(2019, 8, 5), ScheduledEndTime = new DateTime(2019, 8, 5).AddMinutes(5), OptimizationId = Guid.Parse("3908bed6-4469-4bdf-96b9-b477e9a96479") },
            new CncPlan() { Id = 10008, WorkStationId = 10002, ConcreteFurnitureComponentId = 10002, ScheduledStartTime = new DateTime(2019, 8, 5), ScheduledEndTime = new DateTime(2019, 8, 5).AddMinutes(5), OptimizationId = Guid.Parse("3908bed6-4469-4bdf-96b9-b477e9a96479") },

            new CncPlan() 
            { 
                Id = 10017,
                OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                ScheduledStartTime = new DateTime(2020, 2, 1, 9, 0, 0),
                ScheduledEndTime = new DateTime(2020, 2, 1, 14, 0, 0),
                ConcreteFurnitureComponentId = 10003,
                WorkStationId = 10012
            },
            new CncPlan() 
            { 
                Id = 10018,
                OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                ScheduledStartTime = new DateTime(2020, 3, 11, 8, 0, 0),
                ScheduledEndTime = new DateTime(2020, 3, 11, 13, 0, 0),
                ConcreteFurnitureComponentId = 10003,
                WorkStationId = 10012
            }
        };
    }
}
