using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class ContractingPartyDto
    {
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AddressDetailsDto CompanyAddress { get; set; }

        public ContractingPartyDto(User bookedBy)
        {
            UserName = bookedBy.CurrentVersion.Name;
            CompanyName = bookedBy.Company.Name;
            Email = bookedBy.Email;
            Phone = bookedBy.CurrentVersion.Phone;
            CompanyAddress = new AddressDetailsDto(bookedBy.Company.CurrentVersion.HeadOffice);
        }
    }
}