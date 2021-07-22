using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class StorageTestSeed : IEntitySeed<Storage>
    {
        public Storage[] Entities => new[]
        {
            new Storage("Storage Nr1",10000,null) { Id = 10000 },
            new Storage("Storage Nr2",10000,null) { Id = 10001 },
            new Storage("Storage Nr3",10000,null) { Id = 10002 },
            new Storage("Storage Nr4",10000,null) { Id = 10003 }
        };
    }
}
