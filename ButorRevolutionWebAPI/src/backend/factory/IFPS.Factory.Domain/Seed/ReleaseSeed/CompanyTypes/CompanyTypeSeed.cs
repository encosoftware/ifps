using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CompanyTypeSeed : IEntitySeed<CompanyType>
    {
        public CompanyType[] Entities => new[]
        {
            new CompanyType(CompanyTypeEnum.MyCompany) { Id = 1 },
            new CompanyType(CompanyTypeEnum.PartnerCompany) { Id = 2 },
            new CompanyType(CompanyTypeEnum.SupplierCompany) { Id = 3 },
            new CompanyType(CompanyTypeEnum.BrandCompany) { Id = 4 }
        };
    }
}
