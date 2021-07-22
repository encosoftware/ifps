using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class WorkStationSeed : IEntitySeed<WorkStation>
    {
        public WorkStation[] Entities => new[]
        {
            new WorkStation("Layout WS 01 (seed)", 11, true, 1) { Id = 2, MachineId = 5, SecPerComponent = 150 },
            new WorkStation("Layout WS 02 (seed)", 2, true, 1) { Id = 4, MachineId = 6, SecPerComponent = 200 },

            new WorkStation("CNC WS 01 (seed)", 4, true, 2) { Id = 1, MachineId = 3, SecPerComponent = 70 },
            new WorkStation("CNC WS 02 (seed)", 4, true, 2) { Id = 3,  MachineId = 4, SecPerComponent = 50 },

            new WorkStation("Edging WS 01 (seed)", 4, true, 3) { Id = 5, MachineId = 12, SecPerComponent = 90 },
            new WorkStation("Edging WS 02 (seed)", 4, true, 3) { Id = 6, MachineId = 13, SecPerComponent = 120 },

            new WorkStation("Sorting WS 01 (seed)", 6, true, 4) { Id = 7, MachineId = 8, SecPerComponent = 180 },

            new WorkStation("Assembly WS 01 (seed)", 8, true, 5) { Id = 8, MachineId = 7, SecPerComponent = 80 },

            new WorkStation("Packing WS 01 (seed)", 9, true, 6) { Id = 9, MachineId = 9, SecPerComponent = 200 }
        };
    }
}
