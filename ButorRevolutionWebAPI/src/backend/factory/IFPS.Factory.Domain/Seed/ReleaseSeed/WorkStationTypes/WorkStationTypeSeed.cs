using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class WorkStationTypeSeed : IEntitySeed<WorkStationType>
    {
        public WorkStationType[] Entities => new[]
        {
            new WorkStationType(WorkStationTypeEnum.Layout) { Id = 1 },
            new WorkStationType(WorkStationTypeEnum.Cnc) { Id = 2 },
            new WorkStationType(WorkStationTypeEnum.Edging) { Id = 3 },
            new WorkStationType(WorkStationTypeEnum.Sorting) { Id = 4 },
            new WorkStationType(WorkStationTypeEnum.Assembly) { Id = 5 },
            new WorkStationType(WorkStationTypeEnum.Packing) { Id = 6 },
        };
    }
}
