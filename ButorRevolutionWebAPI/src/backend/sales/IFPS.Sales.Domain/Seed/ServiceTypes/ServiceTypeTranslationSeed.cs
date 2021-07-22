using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class ServiceTypeTranslationSeed : IEntitySeed<ServiceTypeTranslation>
    {
        public ServiceTypeTranslation[] Entities => new[]
        {
            new ServiceTypeTranslation(1, "Shipping", LanguageTypeEnum.EN) { Id = 1 },
            new ServiceTypeTranslation(1, "Szállítás", LanguageTypeEnum.HU) { Id = 2 },
            new ServiceTypeTranslation(2, "Assembly", LanguageTypeEnum.EN) { Id = 3 },
            new ServiceTypeTranslation(2, "Összeszerelés", LanguageTypeEnum.HU) { Id = 4 },
            new ServiceTypeTranslation(3, "Installation", LanguageTypeEnum.EN) { Id = 5 },
            new ServiceTypeTranslation(3, "Beépítés", LanguageTypeEnum.HU) { Id = 6 }
        };
    }
}
