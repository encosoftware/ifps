using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class LayoutPlanSeed : IEntitySeed<LayoutPlan>
    {
        //public LayoutPlan[] Entities => new[]
        //{
        //    new LayoutPlan() { Id = 1, WorkStationId = 2, BoardId = new Guid("fb39bf09-e5e3-49c4-8e11-50782d5a5cad"), ScheduledStartTime = Clock.Now, ScheduledEndTime = Clock.Now.AddMinutes(5), OptimizationId = Guid.Parse("9505d035-3d31-4938-b072-10347c19e569") },
        //    new LayoutPlan() { Id = 2, WorkStationId = 4, BoardId = new Guid("fb39bf09-e5e3-49c4-8e11-50782d5a5cad"), ScheduledStartTime = Clock.Now, ScheduledEndTime = Clock.Now.AddMinutes(5), OptimizationId = Guid.Parse("b65394b2-77d8-4936-bbd4-dda58adf0d71") }
        //};
        public LayoutPlan[] Entities => new LayoutPlan[] { };
    }
}
