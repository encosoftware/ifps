using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CFCProductionStateTranslationSeed : IEntitySeed<CFCProductionStateTranslation>
    {
        public CFCProductionStateTranslation[] Entities => new[]
        {
            new CFCProductionStateTranslation(1, "Start position", LanguageTypeEnum.EN) { Id = 1 },
            new CFCProductionStateTranslation(1, "Kezdő pozíció", LanguageTypeEnum.HU) { Id = 2 },
            new CFCProductionStateTranslation(2, "End position", LanguageTypeEnum.EN) { Id = 3 },
            new CFCProductionStateTranslation(2, "Befejező pozíció", LanguageTypeEnum.HU) { Id = 4 }
        };
    }
}
