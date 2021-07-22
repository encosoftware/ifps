using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class SupplierCompanyDto
    { 
        public string CompanyName { get; set; }
        public AddressDetailsDto SupplierAddress { get; set; }
        public ContactByAllInformation ContactPerson { get; set; }

        public SupplierCompanyDto(Company company)
        {
            CompanyName = company.Name;
            SupplierAddress = new AddressDetailsDto(company.CurrentVersion.HeadOffice);
            ContactPerson = company.CurrentVersion.ContactPerson != null ? new ContactByAllInformation(company.CurrentVersion.ContactPerson) : null;
        }
    }
}
