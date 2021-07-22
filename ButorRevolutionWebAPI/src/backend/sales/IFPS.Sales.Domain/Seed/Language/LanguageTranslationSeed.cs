using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class LanguageTranslationSeed : IEntitySeed<LanguageTranslation>
    {
        public LanguageTranslation[] Entities => new[]
        {
            new LanguageTranslation("Magyar",1,LanguageTypeEnum.HU){Id = 1},
            new LanguageTranslation("Magyar",1,LanguageTypeEnum.EN){Id = 2},
            new LanguageTranslation("English",2,LanguageTypeEnum.HU){Id = 3},
            new LanguageTranslation("English",2,LanguageTypeEnum.EN){Id = 4},
        };
    }
}
