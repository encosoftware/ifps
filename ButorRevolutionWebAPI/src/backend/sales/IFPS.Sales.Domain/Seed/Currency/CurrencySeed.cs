using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
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
