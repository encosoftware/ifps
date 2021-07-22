using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class StorageDetailsDto
    {
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public AddressDetailsDto Address { get; set; }

        public StorageDetailsDto(Storage storage)
        {
            CompanyName = storage.Company.Name;
            Name = storage.Name;
            Address = new AddressDetailsDto(storage.Address);
        }
    }
}
