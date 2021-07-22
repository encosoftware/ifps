using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class DayTypeTranslationSeed : IEntitySeed<DayTypeTranslation>
    {        
        public DayTypeTranslation[] Entities => new[]
        {
            new DayTypeTranslation(1,"Hétfő", LanguageTypeEnum.HU){ Id = 1 },
            new DayTypeTranslation(1,"Monday", LanguageTypeEnum.EN){ Id = 2 },
            new DayTypeTranslation(2,"Kedd", LanguageTypeEnum.HU){ Id = 3 },
            new DayTypeTranslation(2,"Tuesday", LanguageTypeEnum.EN){ Id = 4 },
            new DayTypeTranslation(3,"Szerda", LanguageTypeEnum.HU){ Id = 5 },
            new DayTypeTranslation(3,"Wednesday", LanguageTypeEnum.EN){ Id = 6 },
            new DayTypeTranslation(4,"Csütörtök", LanguageTypeEnum.HU){ Id = 7 },
            new DayTypeTranslation(4,"Thursday", LanguageTypeEnum.EN){ Id = 8 },
            new DayTypeTranslation(5,"Péntek", LanguageTypeEnum.HU){ Id = 9 },
            new DayTypeTranslation(5,"Friday", LanguageTypeEnum.EN){ Id = 10 },
            new DayTypeTranslation(6,"Szombat", LanguageTypeEnum.HU){ Id = 11 },
            new DayTypeTranslation(6,"Saturday", LanguageTypeEnum.EN){ Id = 12 },
            new DayTypeTranslation(7,"Vasárnap", LanguageTypeEnum.HU){ Id = 13 },
            new DayTypeTranslation(7,"Sunday", LanguageTypeEnum.EN){ Id = 14 },
            new DayTypeTranslation(8,"Hétköznap", LanguageTypeEnum.HU){ Id = 15 },
            new DayTypeTranslation(8,"Weekdays", LanguageTypeEnum.EN){ Id = 16 },
            new DayTypeTranslation(9,"Hétvége", LanguageTypeEnum.HU){ Id = 17 },
            new DayTypeTranslation(9,"Weekend", LanguageTypeEnum.EN){ Id = 18 }
        };
    }
}
