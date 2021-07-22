using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class CompanyDataTestSeed : IEntitySeed<CompanyData>
    {
        public CompanyData[] Entities => new[]
        {
            new CompanyData( "1111", "1111", null, null, Clock.Now ) { Id = 10000, ContactPersonId = 10004 },
            new CompanyData( "1111", "1111", null, null, Clock.Now ) { Id = 10001 }
        };
    }
}