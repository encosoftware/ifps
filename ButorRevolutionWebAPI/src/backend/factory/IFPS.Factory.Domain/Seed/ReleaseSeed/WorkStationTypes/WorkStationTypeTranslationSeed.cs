using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class WorkStationTypeTranslationSeed : IEntitySeed<WorkStationTypeTranslation>
    {
        public WorkStationTypeTranslation[] Entities => new[]
        {
            new WorkStationTypeTranslation(1, "Cnc workstation", LanguageTypeEnum.EN) { Id = 1 },
            new WorkStationTypeTranslation(1, "Cnc munkaállomás", LanguageTypeEnum.HU) { Id = 2 },
            new WorkStationTypeTranslation(2, "Cutting work station", LanguageTypeEnum.EN) { Id = 3 },
            new WorkStationTypeTranslation(2, "Lapszabász munkaállomás", LanguageTypeEnum.HU) { Id = 4 },
            new WorkStationTypeTranslation(3, "Assembly work station", LanguageTypeEnum.EN) { Id = 5 },
            new WorkStationTypeTranslation(3, "Összeszerelő munkaállomás", LanguageTypeEnum.HU) { Id = 6 },
            new WorkStationTypeTranslation(4, "Edging work station", LanguageTypeEnum.EN) { Id = 7 },
            new WorkStationTypeTranslation(4, "Élfoliázó munkaállomás", LanguageTypeEnum.HU) { Id = 8 },
            new WorkStationTypeTranslation(5, "Sorting work station", LanguageTypeEnum.EN) { Id = 9 },
            new WorkStationTypeTranslation(5, "Szortírozó munkaállomás", LanguageTypeEnum.HU) { Id = 10 },
            new WorkStationTypeTranslation(6, "Packing work station", LanguageTypeEnum.EN) { Id = 11 },
            new WorkStationTypeTranslation(6, "Csomagoló munkaállomás", LanguageTypeEnum.HU) { Id = 12 }
        };

    }
}
