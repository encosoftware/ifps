using IFPS.Factory.Domain.Constants;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class StorageCellSeed : IEntitySeed<StorageCell>
    {
        public StorageCell[] Entities => new[]
        {
            new StorageCell("Column_1 / Row_1", 1) { Id = StorageConstant.FoilMachineStorageCellId, Metadata = "Lorem ipsum dolor sit amet" },
            new StorageCell("Column_1 / Row_2", 1) { Id = StorageConstant.DecorBoardMachineStorageCellId, Metadata = "Lorem ipsum dolor sit amet" },
            new StorageCell("Column_1 / Row_3", 1) { Id = StorageConstant.AccessoryMachineStorageCellId, Metadata = "Lorem ipsum dolor sit amet" },
            new StorageCell("Column_2 / Row_1", 1) { Id = StorageConstant.WorktopBoardMachineStorageCellId, Metadata = "Lorem ipsum dolor sit amet" },
            new StorageCell("Column_2 / Row_2", 1) { Id = 5, Metadata = "Lorem ipsum dolor sit amet" },
            new StorageCell("Column_3 / Row_1", 1) { Id = 6, Metadata = "Lorem ipsum dolor sit amet" }
        };
    }
}
