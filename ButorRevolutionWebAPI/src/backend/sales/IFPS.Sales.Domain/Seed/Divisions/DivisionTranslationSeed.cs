using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class DivisionTranslationSeed : IEntitySeed<DivisionTranslation>
    {        
        public DivisionTranslation[] Entities => new[]
        {
            new DivisionTranslation(1,"Admin modul","Admin modul jogosultságai", LanguageTypeEnum.HU){ Id = 1 },
            new DivisionTranslation(1,"Admin module","Claims of admin module",LanguageTypeEnum.EN){ Id = 2 },

            new DivisionTranslation(2,"Értékesítői modul","Értékesítői modul jogosultságai",LanguageTypeEnum.HU){ Id = 3 },
            new DivisionTranslation(2,"Sales module","Claims of sales module",LanguageTypeEnum.EN){ Id = 4 },

            new DivisionTranslation(3,"Partner modul","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.HU){ Id = 5 },
            new DivisionTranslation(3,"Partner module","Claims of partner module",LanguageTypeEnum.EN){ Id = 6 },

            new DivisionTranslation(4,"Megrendelői modul","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.HU){ Id = 7 },
            new DivisionTranslation(4,"Customer module","Claims of customer module",LanguageTypeEnum.EN){ Id = 8 }
        };
    }
}
