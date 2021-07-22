using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CompanyTypeListDto
    {
        public int Id { get; set; }
        public CompanyTypeEnum CompanyType { get; set; }
        public string Translation { get; set; }

        public CompanyTypeListDto(CompanyType companyType)
        {
            Id = companyType.Id;
            CompanyType = companyType.Type;
            Translation = companyType.CurrentTranslation?.Name;
        }
    }
}