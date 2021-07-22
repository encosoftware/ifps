using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class EdgingPlanTestSeed : IEntitySeed<Plan>
    {
        public Plan[] Entities => new[]
        {
            new Plan() { Id = 10004, ConcreteFurnitureComponentId = 10000, WorkStationId= 10006, ScheduledStartTime = new DateTime(2019, 8, 5), ScheduledEndTime = new DateTime(2019, 8, 5).AddMinutes(5), OptimizationId = Guid.Parse("3908bed6-4469-4bdf-96b9-b477e9a96479") },
            new Plan() { Id = 10005, ConcreteFurnitureComponentId = 10001, WorkStationId= 10006, ScheduledStartTime = new DateTime(2019, 8, 5), ScheduledEndTime = new DateTime(2019, 8, 5).AddMinutes(5), OptimizationId = Guid.Parse("3908bed6-4469-4bdf-96b9-b477e9a96479") },
        
            new Plan()
            {
                Id = 10019,
                OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                ScheduledStartTime = new DateTime(2020, 2, 2, 10, 0, 0),
                ScheduledEndTime = new DateTime(2020, 2, 2, 12, 0, 0),
                ConcreteFurnitureComponentId = 10003,
                WorkStationId = 10013
            },
            new Plan()
            {
                Id = 10020,
                OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                ScheduledStartTime = new DateTime(2020, 3, 12, 8, 0, 0),
                ScheduledEndTime = new DateTime(2020, 3, 12, 11, 0, 0),
                ConcreteFurnitureComponentId = 10003,
                WorkStationId = 10013
            }

        };
    }
}
