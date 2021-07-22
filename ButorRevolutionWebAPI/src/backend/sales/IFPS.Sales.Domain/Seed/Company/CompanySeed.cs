using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class CompanySeed : IEntitySeed<Company>
    {
        public Company[] ReleaseEntities => new[]
        {
            new Company("EN-CO Software (seed)", 1) { Id = 1, CurrentVersionId = 1 }
        };

        public Company[] Entities => new[]
        {
            new Company("Bosch (seed)", 3) { Id = 2, CurrentVersionId = 2 },
            new Company("Zanussi (seed)", 3) {Id = 3, CurrentVersionId = 3},
            new Company("Whirlpool (seed)", 3) {Id = 4, CurrentVersionId = 4},
            new Company("Electrolux (seed)", 3) {Id = 5, CurrentVersionId = 5},
            new Company("AEG (seed)", 3) {Id = 6, CurrentVersionId = 6},
        };
    }
}
