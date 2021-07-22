using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class StorageSeed : IEntitySeed<Storage>
    {
        public Storage[] Entities => new[]
        {
            new Storage("Storage Nr1",1,null) { Id = 1 },
            new Storage("Storage Nr2",1,null) { Id = 2 }
        };
    }
}
