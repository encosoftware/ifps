using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.CompanyTypes
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
            new CompanyTypeTranslation(4,"Viszonteladó cég", LanguageTypeEnum.HU){ Id = 7 },
            new CompanyTypeTranslation(4,"Resail company", LanguageTypeEnum.EN){ Id = 8 },
            new CompanyTypeTranslation(5,"Megrendelő cég", LanguageTypeEnum.HU){ Id = 9 },
            new CompanyTypeTranslation(5,"Megrendelő company", LanguageTypeEnum.EN){ Id = 10 }
        };
    }
}
