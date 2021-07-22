using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class StorageCellDetailsDto
    {
        public string Name { get; set; }
        public int StorageId { get; set; }
        public string Description { get; set; }

        public StorageCellDetailsDto(StorageCell storageCell)
        {
            Name = storageCell.Name;
            StorageId = storageCell.WarehouseId;
            Description = storageCell.Metadata;
        }
    }
}
