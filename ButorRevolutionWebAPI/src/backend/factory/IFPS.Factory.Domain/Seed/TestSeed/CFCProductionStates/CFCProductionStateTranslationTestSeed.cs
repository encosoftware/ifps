using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CFCProductionStateTranslationTestSeed : IEntitySeed<CFCProductionStateTranslation>
    {
        public CFCProductionStateTranslation[] Entities => new[]
        {
            new CFCProductionStateTranslation(10000, "Start position", LanguageTypeEnum.EN) { Id = 10000 },
            new CFCProductionStateTranslation(10000, "Kezdő pozíció", LanguageTypeEnum.HU) { Id = 10001 },
            new CFCProductionStateTranslation(10001, "End position", LanguageTypeEnum.EN) { Id = 10002 },
            new CFCProductionStateTranslation(10001, "Befejező pozíció", LanguageTypeEnum.HU) { Id = 10003 }
        };
    }
}
