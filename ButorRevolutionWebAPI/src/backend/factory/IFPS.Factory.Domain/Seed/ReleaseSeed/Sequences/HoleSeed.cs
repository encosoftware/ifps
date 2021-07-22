using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class HoleSeed : IEntitySeed<Hole>
    {
        public Hole[] Entities => new[]
        {
            new Hole(42) { Id = 2, DrillId = 2 },
            new Hole(42) { Id = 3, DrillId = 3 },
            new Hole(42) { Id = 4, DrillId = 4 },
            new Hole(42) { Id = 5, DrillId = 4 },
            new Hole(42) { Id = 6, DrillId = 5 },
            new Hole(42) { Id = 7, DrillId = 5 },
            new Hole(42) { Id = 8, DrillId = 6 },
            new Hole(42) { Id = 9, DrillId = 6 },
            new Hole(42) { Id = 10, DrillId = 7 },
            new Hole(42) { Id = 11, DrillId = 7 },
            new Hole(42) { Id = 12, DrillId = 8 },
            new Hole(42) { Id = 13, DrillId = 8 }
        };

        //public Hole[] Entities => new Hole[] { };
    }
}
