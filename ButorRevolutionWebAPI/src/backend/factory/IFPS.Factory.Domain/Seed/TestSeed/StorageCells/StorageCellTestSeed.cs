using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class StorageCellTestSeed : IEntitySeed<StorageCell>
    {
        public StorageCell[] Entities => new[]
        {
            new StorageCell("column3 / row2",10000) { Id = 10000, Metadata = "Desc1" },
            new StorageCell("column3 / row4",10000) { Id = 10001, Metadata = "Desc2" },
            new StorageCell("column3 / row6",10000) { Id = 10002, Metadata = "Desc3" },
            new StorageCell("column3 / row8",10000) { Id = 10003, Metadata = "Desc4" }
        };
    }
}