using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CompanySeed : IEntitySeed<Company>
    {
        public Company[] ReleaseEntities => new[]
        {
            new Company("ENCO Software (seed)", 1) { Id = 1, CurrentVersionId = 1 },
            new Company("Supplier Company 01 (seed)", 3) { Id = 2, CurrentVersionId = 1 },
            new Company("Supplier Company 02 (seed)", 3) { Id = 3, CurrentVersionId = 1 },
            new Company("Jysk (seed)", 3) { Id = 4, CurrentVersionId = 1 },
            new Company("Machine Ltd. (seed)", 2) { Id = 5, CurrentVersionId = 2 },

            new Company("Bosch (seed)", 3) { Id = 6, CurrentVersionId = 3 },
            new Company("Zanussi (seed)", 3) {Id = 7, CurrentVersionId = 4},
            new Company("Whirlpool (seed)", 3) {Id = 8, CurrentVersionId = 5},
            new Company("Electrolux (seed)", 3) {Id = 9, CurrentVersionId = 6},
            new Company("AEG (seed)", 3) {Id = 10, CurrentVersionId = 7}
        };

        public Company[] Entities => new Company[] { };
    }
}
