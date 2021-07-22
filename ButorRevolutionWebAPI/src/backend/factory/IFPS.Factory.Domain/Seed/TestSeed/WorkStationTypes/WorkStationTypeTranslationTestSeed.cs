using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class WorkStationTypeTranslationTestSeed : IEntitySeed<WorkStationTypeTranslation>
    {
        public WorkStationTypeTranslation[] Entities => new[]
        {
            new WorkStationTypeTranslation(10000, "Cnc work station", LanguageTypeEnum.EN) { Id = 10000 },
            new WorkStationTypeTranslation(10000, "Cnc munkaállomás", LanguageTypeEnum.HU) { Id = 10001 },
            new WorkStationTypeTranslation(10001, "Cutting work station", LanguageTypeEnum.EN) { Id = 10002 },
            new WorkStationTypeTranslation(10001, "Vágóhely", LanguageTypeEnum.HU) { Id = 10003 },
            new WorkStationTypeTranslation(10002, "Assembly work station", LanguageTypeEnum.EN) { Id = 10004 },
            new WorkStationTypeTranslation(10002, "Összeszerelő műhely", LanguageTypeEnum.HU) { Id = 10005 }
        };

    }
}
