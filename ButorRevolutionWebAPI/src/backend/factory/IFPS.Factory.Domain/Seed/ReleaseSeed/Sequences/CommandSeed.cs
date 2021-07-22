using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CommandSeed : IEntitySeed<Command>
    {
        public Command[] Entites => new[]
        {
            new Command(42) { Id = 1, SequenceId = 1 }
        };
        //public Command[] Entities => new Command[] { };
    }
}
