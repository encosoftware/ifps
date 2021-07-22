using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class CompanyDataSeed : IEntitySeed<CompanyData>
    {
        public CompanyData[] ReleaseEntities => new[]
        {
            new CompanyData("6546546565-05", "41110500556", null, 7, new DateTime(2019, 8, 18) ) { Id = 1 },
            new CompanyData("6546546565-06", "41110500556", null, 7, new DateTime(2019, 8, 18) ) { Id = 2 },

            new CompanyData("6546546565-05", "41110500556", null, 7, new DateTime(2019, 8, 18)) { Id = 3 },
            new CompanyData("6546546565-05", "41110500556", null, 7, new DateTime(2019, 8, 18)) { Id = 4 },
            new CompanyData("4857125242-02", "14011465830", null, 7, new DateTime(2019, 8, 18)) { Id = 5 },
            new CompanyData("8468335005-02", "65498714654", null, 7, new DateTime(2019, 8, 18)) { Id = 6 },
            new CompanyData("5786011910-02", "11026544604", null, 7, new DateTime(2019, 8, 18)) { Id = 7 }
        };

        public CompanyData[] Entities => new CompanyData[] { };
    }
}
