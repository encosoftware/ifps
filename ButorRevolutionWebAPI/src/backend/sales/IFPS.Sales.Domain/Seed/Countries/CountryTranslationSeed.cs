using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.Countries
{
    public class CountryTranslationSeed : IEntitySeed<CountryTranslation>
    {
        public CountryTranslation[] Entities => new[]
        {
            new CountryTranslation (1, "Magyarország",  LanguageTypeEnum.HU) { Id = 1},
            new CountryTranslation (1, "Hungary",       LanguageTypeEnum.EN) { Id = 2 },
            new CountryTranslation (2, "Szlovákia",     LanguageTypeEnum.HU) { Id = 3},                
            new CountryTranslation (2, "Slovakia",      LanguageTypeEnum.EN) { Id = 4 }
        };
    }
}
