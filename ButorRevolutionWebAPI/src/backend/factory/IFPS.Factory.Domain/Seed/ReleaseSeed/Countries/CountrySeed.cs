using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CountrySeed : IEntitySeed<Country>
    {
        public Country[] Entities => new[]
        {
            new Country("HU") { Id = 1},
            new Country("SK") { Id = 2}
        };
    }
}
