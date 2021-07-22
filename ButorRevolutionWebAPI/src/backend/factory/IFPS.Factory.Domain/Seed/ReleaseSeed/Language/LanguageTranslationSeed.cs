using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class LanguageTranslationSeed : IEntitySeed<LanguageTranslation>
    {
        public LanguageTranslation[] Entities => new[]
        {
            new LanguageTranslation("Magyar",1,LanguageTypeEnum.HU){Id = 1},
            new LanguageTranslation("Hungarian",1,LanguageTypeEnum.EN){Id = 2},
            new LanguageTranslation("Angol",2,LanguageTypeEnum.HU){Id = 3},
            new LanguageTranslation("English",2,LanguageTypeEnum.EN){Id = 4},
        };
    }
}
