using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class CompanyDataSeed : IEntitySeed<CompanyData>
    {
        public CompanyData[] ReleaseEntities => new[]
        {
            new CompanyData("1111", "1111", null,null, Clock.Now) {Id = 1}
        };

        public CompanyData[] Entities => new[]
        {
            new CompanyData("1111", "1111", null,null, Clock.Now) {Id = 2},
            new CompanyData("6546546565-05", "41110500556", null, null, Clock.Now) { Id = 3 },
            new CompanyData("4857125242-02", "14011465830", null, null, Clock.Now) { Id = 4 },
            new CompanyData("8468335005-02", "65498714654", null, null, Clock.Now) { Id = 5 },
            new CompanyData("5786011910-02", "11026544604", null, null, Clock.Now) { Id = 6 },
        };
    }
}
