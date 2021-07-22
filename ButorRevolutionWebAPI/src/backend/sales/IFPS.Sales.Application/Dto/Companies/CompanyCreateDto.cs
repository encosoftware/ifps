using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class CompanyCreateDto
    {
        public string Name { get; set; }
        public int CompanyTypeId { get; set; }
        public string TaxNumber { get; set; }
        public string RegisterNumber { get; set; }
        public AddressCreateDto Address { get; set; }

        public CompanyCreateDto()
        {

        }

        public Company CreateModelObject()
        {
            return new Company(Name, CompanyTypeId, TaxNumber, RegisterNumber, Address.CreateModelObject(), null);
        }
    }
}
