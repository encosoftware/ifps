using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class OptimizationTestSeed : IEntitySeed<Optimization>
    {
        public Optimization[] Entities => new[]
        {
            new Optimization(5, 14) { Id = Guid.Parse("3908bed6-4469-4bdf-96b9-b477e9a96479") }
        };
    }
}
