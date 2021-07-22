using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class OptimizationSeed : IEntitySeed<Optimization>
    {
        public Optimization[] Entities => new[]
        {
            new Optimization(2,8) { Id = new Guid("9505d035-3d31-4938-b072-10347c19e569"), CreationTime = new DateTime(2019, 12, 01, 14, 50, 00) },
            new Optimization(1,12) { Id = new Guid("b65394b2-77d8-4936-bbd4-dda58adf0d71"), CreationTime = new DateTime(2019, 12, 02, 11, 30, 00) },
        };

        //public Optimization[] Entities => new Optimization[] { };
    }
}
