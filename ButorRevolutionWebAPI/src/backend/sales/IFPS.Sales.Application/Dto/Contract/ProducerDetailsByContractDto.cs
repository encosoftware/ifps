using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class ProducerDetailsByContractDto
    {
        public string Name { get; set; }
        public AddressDetailsDto CorrespondentAddress { get; set; }
        public string TaxNumber { get; set; }
        public string BankAccount { get; set; }

        public ProducerDetailsByContractDto(SalesPerson salesPerson)
        {
            Name = salesPerson.User.Company.Name;
            CorrespondentAddress = new AddressDetailsDto(salesPerson.User.Company.CurrentVersion.HeadOffice);
            TaxNumber = salesPerson.User.Company.CurrentVersion.TaxNumber;
        }
    }
}
