using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class AddressCreateDto
    {
        public string Address { get; set; }
        public int PostCode { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }

        public AddressCreateDto() { }

        public Address CreateModelObject()
        {
            return new Address(PostCode, City, Address, CountryId);
        }
    }
}
