using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CompanyTypeTestSeed : IEntitySeed<CompanyType>
    {
        public CompanyType[] Entities => new[]
        {
            new CompanyType(CompanyTypeEnum.MyCompany) { Id = 10000 },
            new CompanyType(CompanyTypeEnum.PartnerCompany) { Id = 10001 },
            new CompanyType(CompanyTypeEnum.SupplierCompany) { Id = 10002 }
        };
    }
}