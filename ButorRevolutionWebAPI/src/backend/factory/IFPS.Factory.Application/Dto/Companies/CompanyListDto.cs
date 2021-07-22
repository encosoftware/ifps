using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class CompanyListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CompanyTypeListDto CompanyType { get; set; }
        public AddressDetailsDto Address { get; set; }
        public ContactPersonListDto ContactPerson { get; set; }

        public CompanyListDto(Company company)
        {
            Id = company.Id;
            Name = company.Name;
            Address = new AddressDetailsDto(company.CurrentVersion.HeadOffice);
            ContactPerson = new ContactPersonListDto(company.CurrentVersion.ContactPerson);
            CompanyType = new CompanyTypeListDto(company.CompanyType);
        }

        public CompanyListDto()
        {
        }

        public static Func<Company, CompanyListDto> FromEntity = entity => new CompanyListDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = new AddressDetailsDto(entity.CurrentVersion.HeadOffice),
            ContactPerson = new ContactPersonListDto(entity.CurrentVersion.ContactPerson),
            CompanyType = new CompanyTypeListDto(entity.CompanyType)
        };

        public static Expression<Func<Company, CompanyListDto>> Projection
        {
            get
            {
                return x => new CompanyListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = new AddressDetailsDto(x.CurrentVersion.HeadOffice),
                    ContactPerson = new ContactPersonListDto(x.CurrentVersion.ContactPerson),
                    CompanyType = new CompanyTypeListDto(x.CompanyType)
                };
            }
        }
    }
}