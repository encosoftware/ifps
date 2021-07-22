using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class StorageCellCreateDto
    {
        public string Name { get; set; }
        public int StorageId { get; set; }
        public string Description { get; set; }

        public StorageCell CreateModelObject()
        {
            return new StorageCell(Name, StorageId) { Metadata = Description };
        }
    }
}
