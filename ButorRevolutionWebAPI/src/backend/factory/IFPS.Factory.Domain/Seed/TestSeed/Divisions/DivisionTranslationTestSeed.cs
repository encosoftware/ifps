using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class DivisionTranslationTestSeed : IEntitySeed<DivisionTranslation>
    {        
        public DivisionTranslation[] Entities => new[]
        {
            new DivisionTranslation(10000,"Admin modul","Admin modul jogosultságai", LanguageTypeEnum.HU){ Id = 10000 },
            new DivisionTranslation(10000,"Admin module","Claims of admin module",LanguageTypeEnum.EN){ Id = 10001 },

            new DivisionTranslation(10001,"Gyártói modul","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.HU){ Id = 10002 },
            new DivisionTranslation(10001,"Production module","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.EN){ Id = 10003 },

            new DivisionTranslation(10002,"Pénzügyi modul","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.HU){ Id = 10004 },
            new DivisionTranslation(10002,"Financial module","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.EN){ Id = 10005 },

            new DivisionTranslation(10003,"Beszállítói modul","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.HU){ Id = 10006 },
            new DivisionTranslation(10003,"Supply module","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.EN){ Id = 10007 },

            new DivisionTranslation(10004,"Raktározói modul","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.HU){ Id = 10008 },
            new DivisionTranslation(10004,"Warehouse module","Törzsadatok felvételéért felelős jogosultságok",LanguageTypeEnum.EN){ Id = 10009 },
        };
    }
}
