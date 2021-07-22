using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.Countries
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
