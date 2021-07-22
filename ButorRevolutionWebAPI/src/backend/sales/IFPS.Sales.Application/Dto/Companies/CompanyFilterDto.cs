using ENCO.DDD.Application.Dto;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class CompanyFilterDto : OrderedPagedRequestDto
    {
        public string Name { get; set; }
        public CompanyTypeEnum CompanyType { get; set; }
        public string ContactPersonEmail { get; set; }
        public string Address { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
                {
                    { nameof(Name), nameof(Company.Name) },
                    { nameof(CompanyType), nameof(Company.CompanyType) },
                    { nameof(Address), nameof(Company.CurrentVersion.HeadOffice.AddressValue) }
              };
        }
    }
}
