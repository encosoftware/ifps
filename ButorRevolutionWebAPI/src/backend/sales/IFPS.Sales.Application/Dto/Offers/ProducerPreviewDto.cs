using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class ProducerPreviewDto
    {
        public string CompanyName { get; set; }
        public AddressDetailsDto Address { get; set; }
        public string ContactPersonName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ProducerPreviewDto(SalesPerson salesPerson)
        {
            CompanyName = salesPerson.User.Company.Name;
            Address = new AddressDetailsDto(salesPerson.User.Company.CurrentVersion.HeadOffice);
            ContactPersonName = salesPerson.User.CurrentVersion.Name;
            Email = salesPerson.User.Email;
            Phone = salesPerson.User.CurrentVersion.Phone;
        }
    }
}
