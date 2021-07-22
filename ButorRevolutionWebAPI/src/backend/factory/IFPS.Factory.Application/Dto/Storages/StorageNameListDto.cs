using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class StorageNameListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public StorageNameListDto(Storage storage)
        {
            Id = storage.Id;
            Name = storage.Name;
        }
    }
}
