using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class WorkStationTestSeed : IEntitySeed<WorkStation>
    {
        public WorkStation[] Entities => new[]
        {
            new WorkStation("First Test WS", 4, true, 10000) { Id = 10000,  MachineId = 10002 },
            new WorkStation("Second Test WS", 11, true, 10001) { Id = 10001,  MachineId = 10006 },
            new WorkStation("Third Test WS", 42, true, 10000) { Id = 10002,  MachineId = 10003 },
            new WorkStation("Fourth Test WS", 8, true, 10001) { Id = 10003,  MachineId = 10007 },
            new WorkStation("Fifth DEL WS", 3, true, 10000) { Id = 10004,  MachineId = 10003 },
            new WorkStation("Sixth UPDATE WS", 1, true, 10001) { Id = 10005,  MachineId = 10007 },
            new WorkStation("7 UPDATE WS", 1, true, 10003) { Id = 10006,  MachineId = 10000 },
            new WorkStation("8 UPDATE WS", 1, true, 10001) { Id = 10007,  MachineId = 10001 },

            // ProductionProcess/Plan
            new WorkStation("Assembly WS PP", 1, true, 10002) { Id = 10008,  MachineId = 10001 },
            new WorkStation("Packing WS PP", 1, true, 10005) { Id = 10009,  MachineId = 10001 },
            new WorkStation("Sorting WS PP", 1, true, 10004) { Id = 10010,  MachineId = 10001 },
            new WorkStation("Layout WS PP", 1, true, 10001) { Id = 10011,  MachineId = 10001 },
            new WorkStation("Cnc WS PP", 1, true, 10000) { Id = 10012,  MachineId = 10001 },
            new WorkStation("Edgebanding WS PP", 1, true, 10003) { Id = 10013,  MachineId = 10001 }
        };
    }
}
