using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class StorageCellUpdateDto
    {
        public string Name { get; set; }
        public int StorageId { get; set; }
        public string Description { get; set; }

        public StorageCell UpdateModelObject(StorageCell storageCell)
        {
            storageCell.Name = Name;
            storageCell.WarehouseId = StorageId;
            storageCell.Metadata = Description;
            return storageCell;
        }
    }
}
