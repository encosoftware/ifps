using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class FrequencyTypeTranslationsSeed : IEntitySeed<FrequencyTypeTranslation>
    {
        public FrequencyTypeTranslation[] Entities => new[]
        {
            new FrequencyTypeTranslation("Nap", 1, LanguageTypeEnum.HU) { Id = 1 },
            new FrequencyTypeTranslation("Day", 1, LanguageTypeEnum.EN) { Id = 2 },
            new FrequencyTypeTranslation("Hét", 2, LanguageTypeEnum.HU) { Id = 3 },
            new FrequencyTypeTranslation("Week", 2, LanguageTypeEnum.EN) { Id = 4 },
            new FrequencyTypeTranslation("Hónap", 3, LanguageTypeEnum.HU) { Id = 5 },
            new FrequencyTypeTranslation("Month", 3, LanguageTypeEnum.EN) { Id = 6 },
            new FrequencyTypeTranslation("Év", 4, LanguageTypeEnum.HU) { Id = 7 },
            new FrequencyTypeTranslation("Year", 4, LanguageTypeEnum.EN) { Id = 8 }            
        };
    }
}
