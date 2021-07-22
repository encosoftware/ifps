using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.CompanyTypes
{
    public class CompanyTypeSeed : IEntitySeed<CompanyType>
    {
        public CompanyType[] Entities => new[]
        {
            new CompanyType(CompanyTypeEnum.MyCompany) {Id = 1},
            new CompanyType(CompanyTypeEnum.PartnerCompany) {Id = 2},
            new CompanyType(CompanyTypeEnum.SupplierCompany) {Id = 3},
            new CompanyType(CompanyTypeEnum.ResailCompany) {Id = 4},
            new CompanyType(CompanyTypeEnum.CustomerCompany) {Id = 5}
        };
    }
}
