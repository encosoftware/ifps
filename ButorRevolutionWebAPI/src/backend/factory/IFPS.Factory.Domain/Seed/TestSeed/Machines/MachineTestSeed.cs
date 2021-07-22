using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class MachineTestSeed : IEntitySeed<Machine>
    {
        public Machine[] Entities => new[]
        {
            new Machine("Test Strong machine", 10004) { Id = 10000 },
            new Machine("The Test strongest machine", 10004) { Id = 10001 }
        };
    }
}
