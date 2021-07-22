using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CompanyTypeTranslationSeed : IEntitySeed<CompanyTypeTranslation>
    {        
        public CompanyTypeTranslation[] Entities => new[]
        {
            new CompanyTypeTranslation(1,"Tulajdonos", LanguageTypeEnum.HU){ Id = 1 },
            new CompanyTypeTranslation(1,"Owner company", LanguageTypeEnum.EN){ Id = 2 },
            new CompanyTypeTranslation(2,"Partner cég", LanguageTypeEnum.HU){ Id = 3 },
            new CompanyTypeTranslation(2,"Partner company", LanguageTypeEnum.EN){ Id = 4 },
            new CompanyTypeTranslation(3,"Beszállító cég", LanguageTypeEnum.HU){ Id = 5 },
            new CompanyTypeTranslation(3,"Supplier company", LanguageTypeEnum.EN){ Id = 6 },
            new CompanyTypeTranslation(4,"Márka", LanguageTypeEnum.HU){ Id = 7 },
            new CompanyTypeTranslation(4,"Brand company", LanguageTypeEnum.EN){ Id = 8 }
        };
    }
}
