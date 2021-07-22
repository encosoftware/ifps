using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class LayoutPlanTestSeed : IEntitySeed<LayoutPlan>
    {
        public LayoutPlan[] Entities => new[]
        {
            new LayoutPlan() { Id = 10000, WorkStationId = 10006, ConcreteFurnitureComponentId = 10000, ScheduledStartTime = new DateTime(2019, 8, 5), ScheduledEndTime = new DateTime(2019, 8, 5).AddMinutes(5), OptimizationId = Guid.Parse("3908bed6-4469-4bdf-96b9-b477e9a96479") },
            new LayoutPlan() { Id = 10001, WorkStationId = 10007, ConcreteFurnitureComponentId = 10001, ScheduledStartTime = new DateTime(2019, 8, 5), ScheduledEndTime = new DateTime(2019, 8, 5).AddMinutes(5), OptimizationId = Guid.Parse("3908bed6-4469-4bdf-96b9-b477e9a96479") },

            new LayoutPlan()
            {
                Id = 10015,
                OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                ScheduledStartTime = new DateTime(2020, 1, 31, 12, 0, 0),
                ScheduledEndTime = new DateTime(2020, 1, 31, 18, 0, 0),
                ConcreteFurnitureComponentId = 10003,
                WorkStationId = 10011,
                BoardId = new Guid("fb39bf09-e5e3-49c4-8e11-50782d5a5cad")
            },
            new LayoutPlan()
            {
                Id = 10016,
                OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                ScheduledStartTime = new DateTime(2020, 3, 10, 8, 0, 0),
                ScheduledEndTime = new DateTime(2020, 3, 10, 15, 0, 0),
                ConcreteFurnitureComponentId = 10003,
                WorkStationId = 10011,
                BoardId = new Guid("fb39bf09-e5e3-49c4-8e11-50782d5a5cad")
            }
        };
    }
}
