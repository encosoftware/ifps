using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class LanguageSeed : IEntitySeed<Language>
    {
        public Language[] Entities => new[]
        {
            new Language(LanguageTypeEnum.HU){ Id = 1},
            new Language(LanguageTypeEnum.EN){ Id = 2},
        };
    }
}
