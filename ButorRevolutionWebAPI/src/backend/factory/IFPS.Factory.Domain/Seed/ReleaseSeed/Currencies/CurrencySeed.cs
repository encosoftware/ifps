using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CurrencySeed : IEntitySeed<Currency>
    {
        public Currency[] Entities => new[]
        {
            new Currency("HUF") {Id = 1},
            new Currency("EUR") {Id = 2}
        };
    }
}
