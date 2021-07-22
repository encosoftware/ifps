using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CargoStatusTypeTranslationTestSeed : IEntitySeed<CargoStatusTypeTranslation>
    {
        public CargoStatusTypeTranslation[] Entities => new[]
        {
            new CargoStatusTypeTranslation(10000, "Megrendelve", LanguageTypeEnum.HU) { Id = 10000 },
            new CargoStatusTypeTranslation(10000, "Ordered", LanguageTypeEnum.EN) { Id = 10001 },
            new CargoStatusTypeTranslation(10001, "Várakozás raktározásra", LanguageTypeEnum.HU) { Id = 10002 },
            new CargoStatusTypeTranslation(10001, "Waiting for stocking", LanguageTypeEnum.EN) { Id = 10003 }
        };
    }
}
