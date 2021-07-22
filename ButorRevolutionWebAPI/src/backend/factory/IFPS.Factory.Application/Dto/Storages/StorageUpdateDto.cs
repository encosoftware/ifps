using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class StorageUpdateDto
    {
        public string Name { get; set; }
        public AddressUpdateDto Address { get; set; }

        public Storage UpdateModelObject(Storage storage)
        {
            storage.Name = Name;
            storage.Address = Address.UpdateModelObject();
            return storage;
        }
    }
}
