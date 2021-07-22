using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class TempAddressCreateDto
    {
        public string Address { get; set; }
        public int PostCode { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }

        public TempAddressCreateDto()
        {
            Address = "Kuruclesi út 4.";
            PostCode = 1122;
            City = "Budapest";
            CountryId = 1;
        }

        public Address CreateModelObject()
        {
            return new Address(PostCode, City, Address, CountryId) { };
        }
    }
}
