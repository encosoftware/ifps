using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CargoStatusTypeTranslationSeed : IEntitySeed<CargoStatusTypeTranslation>
    {
        public CargoStatusTypeTranslation[] Entities => new[]
        {
            new CargoStatusTypeTranslation(1, "Megrendelve", LanguageTypeEnum.HU) { Id = 1 },
            new CargoStatusTypeTranslation(1, "Ordered", LanguageTypeEnum.EN) { Id = 2 },
            new CargoStatusTypeTranslation(2, "Várakozás raktározásra", LanguageTypeEnum.HU) { Id = 3 },
            new CargoStatusTypeTranslation(2, "Waiting for stocking", LanguageTypeEnum.EN) { Id = 4 },
            new CargoStatusTypeTranslation(3, "Raktározva", LanguageTypeEnum.HU) { Id = 5 },
            new CargoStatusTypeTranslation(3, "Stocked", LanguageTypeEnum.EN) { Id = 6 }
        };
    }
}
