using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
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
