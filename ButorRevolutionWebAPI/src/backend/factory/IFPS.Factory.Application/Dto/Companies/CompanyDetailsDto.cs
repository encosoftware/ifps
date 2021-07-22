using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class CompanyDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CompanyTypeListDto CompanyType { get; set; }
        public string TaxNumber { get; set; }
        public string RegisterNumber { get; set; }
        public AddressDetailsDto Address { get; set; }
        public ContactPersonListDto ContactPerson { get; set; }
        public List<CompanyDateRangeDetailsDto> OpeningHours { get; set; }
        public List<UserTeamListDto> UserTeams { get; set; }
        public List<EmployeeListDto> Employees { get; set; }

        public CompanyDetailsDto(Company company)
        {
            Id = company.Id;
            Name = company.Name;
            TaxNumber = company.CurrentVersion.TaxNumber;
            RegisterNumber = company.CurrentVersion.RegisterNumber;
            Address = new AddressDetailsDto(company.CurrentVersion.HeadOffice);
            ContactPerson = new ContactPersonListDto(company.CurrentVersion?.ContactPerson);
            UserTeams = company.UserTeams.Select(ent => UserTeamListDto.FromModel(ent)).ToList();
            OpeningHours = company.OpeningHours.Select(ent => new CompanyDateRangeDetailsDto(ent)).ToList();
            Employees = new List<EmployeeListDto>();
            CompanyType = new CompanyTypeListDto(company.CompanyType);
        }
    }
}