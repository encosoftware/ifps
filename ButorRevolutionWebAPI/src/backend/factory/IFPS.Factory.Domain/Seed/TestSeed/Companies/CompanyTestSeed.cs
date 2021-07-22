using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CompanyTestSeed : IEntitySeed<Company>
    {
        public Company[] Entities => new[]
        {
            new Company("Test EN-CO Software", 10000) { Id = 10000, CurrentVersionId = 10000 },
            new Company("Test Super Supplier Company", 10002) { Id = 10001, CurrentVersionId = 10000 },
            new Company("Super Supplier Company", 10002) { Id = 10002, CurrentVersionId = 10000 },
            new Company("Millenium Falcon Space Supplier Inc.", 10002) { Id = 10003, CurrentVersionId = 10000 },
            new Company("Machine Inc.", 10001) { Id = 10004, CurrentVersionId = 10001 },

            new Company("Test Bosch", 10000) { Id = 10009, CurrentVersionId = 10001 },

        };
    }
}