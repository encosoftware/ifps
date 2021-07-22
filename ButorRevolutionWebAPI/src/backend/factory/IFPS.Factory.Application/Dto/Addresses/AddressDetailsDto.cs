using IFPS.Factory.Application.Exceptions;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class AddressDetailsDto
    {
        public string Address { get; set; }
        public int PostCode { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }

        public AddressDetailsDto() { }

        public AddressDetailsDto(Address address)
        {
            if (!address.CountryId.HasValue)
            {
                throw new IFPSAppException($"No country attached to address, address value: {Address}");
            }

            Address = address.AddressValue;
            PostCode = address.PostCode;
            City = address.City;
            CountryId = address.CountryId.Value;
        }
    }
}
