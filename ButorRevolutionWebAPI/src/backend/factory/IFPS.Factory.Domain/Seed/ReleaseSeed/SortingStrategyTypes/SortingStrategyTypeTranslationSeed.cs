using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class SortingStrategyTypeTranslationSeed : IEntitySeed<SortingStrategyTypeTranslation>
    {
        public SortingStrategyTypeTranslation[] Entities => new[]
        {
            new SortingStrategyTypeTranslation(1, "Sorting by area", LanguageTypeEnum.EN) { Id = 1 },
            new SortingStrategyTypeTranslation(1, "Rendezés terület szerint", LanguageTypeEnum.HU) { Id = 2 },
            new SortingStrategyTypeTranslation(2, "Sorting by order", LanguageTypeEnum.EN) { Id = 3 },
            new SortingStrategyTypeTranslation(2, "Rendezés rendelés szerint", LanguageTypeEnum.HU) { Id = 4 }
        };
    }
}
