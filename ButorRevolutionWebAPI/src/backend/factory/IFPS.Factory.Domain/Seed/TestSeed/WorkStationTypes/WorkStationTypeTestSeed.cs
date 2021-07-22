using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class WorkStationTypeTestSeed : IEntitySeed<WorkStationType>
    {
        public WorkStationType[] Entities => new[]
        {
            new WorkStationType(WorkStationTypeEnum.Cnc) { Id = 10000 },
            new WorkStationType(WorkStationTypeEnum.Layout) { Id = 10001 },
            new WorkStationType(WorkStationTypeEnum.Assembly) { Id = 10002 },
            new WorkStationType(WorkStationTypeEnum.Edging) { Id = 10003 },
            new WorkStationType(WorkStationTypeEnum.Sorting) { Id = 10004 },
            new WorkStationType(WorkStationTypeEnum.Packing) { Id = 10005 }
        };
    }
}
