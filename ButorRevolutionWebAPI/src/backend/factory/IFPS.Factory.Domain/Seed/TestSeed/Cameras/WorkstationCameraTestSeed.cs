using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class WorkstationCameraTestSeed : IEntitySeed<WorkStationCamera>
    {
        public WorkStationCamera[] Entities = new[]
        {
            new WorkStationCamera(10000, 10000, 10000) {Id = 10000 },
            new WorkStationCamera(10001, 10000, 10001) {Id = 10001 },
            new WorkStationCamera(10002, 10002, 10000) {Id = 10002 },
            new WorkStationCamera(10003, 10002, 10001) {Id = 10003 },
            new WorkStationCamera(10004, 10003, 10000) {Id = 10004 },
            new WorkStationCamera(10005, 10003, 10001) {Id = 10005 }
        };
    }
}
