using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class DivisionTranslationSeed : IEntitySeed<DivisionTranslation>
    {
        public DivisionTranslation[] Entities => new[]
        {
            new DivisionTranslation(1,"Admin modul","Admin modul jogosultságai", LanguageTypeEnum.HU){ Id = 1 },
            new DivisionTranslation(1,"Admin module","Claims of admin module",LanguageTypeEnum.EN){ Id = 2 },

            new DivisionTranslation(2,"Gyártói modul","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.HU){ Id = 3 },
            new DivisionTranslation(2,"Production module","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.EN){ Id = 4 },

            new DivisionTranslation(3,"Pénzügyi modul","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.HU){ Id = 5 },
            new DivisionTranslation(3,"Financial module","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.EN){ Id = 6 },

            new DivisionTranslation(4,"Beszerzési modul","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.HU){ Id = 7 },
            new DivisionTranslation(4,"Supply module","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.EN){ Id = 8 },

            new DivisionTranslation(5,"Raktározói modul","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.HU){ Id = 9 },
            new DivisionTranslation(5,"Warehouse module","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.EN){ Id = 10 }
        };
    }
}
