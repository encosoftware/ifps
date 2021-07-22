using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class StorageCreateDto
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public AddressCreateDto Address { get; set; }

        public StorageCreateDto()
        {

        }

        public Storage CreateModelObject()
        {
            return new Storage(Name,CompanyId,Address.CreateModelObject());
        }
    }
}
