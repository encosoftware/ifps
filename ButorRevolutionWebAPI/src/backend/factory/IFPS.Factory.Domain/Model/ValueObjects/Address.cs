using ENCO.DDD.Domain.Model.Values;
using IFPS.Factory.Domain.Exceptions;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class Address : ValueObject<Address>
    {
        /// <summary>
        /// Name and type of street, house number
        /// </summary>
        public string AddressValue { get; private set; }

        /// <summary>
        /// Name of city
        /// </summary>
        public string City { get; private set; }

        /// <summary>
        /// PostCode
        /// </summary>
        public int PostCode { get; private set; }

        /// <summary>
        /// Country associated with the address
        /// </summary>
        public virtual Country Country { get; private set; }
        public int? CountryId { get; private set; }

        /// <summary>
        /// Default constructor is private
        /// </summary>
        private Address()
        {

        }

        public Address(int postCode, string city, string address, int countryId)
        {
            this.PostCode = postCode;
            this.City = city;
            this.AddressValue = address;
            this.CountryId = countryId;

            if (countryId <= 0)
            {
                throw new IFPSDomainException();
            }
        }

        public bool IsEmpty()
        {
            return CountryId == null && String.IsNullOrEmpty(City) && String.IsNullOrEmpty(AddressValue) && PostCode < 0;
        }

        public override string ToString() => $"{PostCode} {City} {AddressValue}";

        public static Address GetEmptyAddress()
        {
            return new Address
            {
                PostCode = -1,
                City = string.Empty,
                AddressValue = string.Empty,
                CountryId = null,
            };
        }
    }
}
