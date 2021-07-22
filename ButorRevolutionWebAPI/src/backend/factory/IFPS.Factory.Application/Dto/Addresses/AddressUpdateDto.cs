using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class AddressUpdateDto
    {
        public string Address { get; set; }

        public int PostCode { get; set; }

        public string City { get; set; }

        public int CountryId { get; set; }

        public AddressUpdateDto()
        {

        }

        public Address UpdateModelObject()
        {
            return new Address(PostCode, City, Address, CountryId);
        }
    }
}
