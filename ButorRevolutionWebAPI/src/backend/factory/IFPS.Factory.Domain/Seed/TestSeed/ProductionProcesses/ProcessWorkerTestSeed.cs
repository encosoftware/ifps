using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class ProcessWorkerTestSeed : IEntitySeed<ProcessWorker>
    {
        public ProcessWorker[] Entities => new[]
        {
            new ProcessWorker() { Id = 10000, ProcessId = 10000, WorkerId = 10004 },
            new ProcessWorker() { Id = 10001, ProcessId = 10000, WorkerId = 10003 },
            new ProcessWorker() { Id = 10002, ProcessId = 10001, WorkerId = 10004 },
            new ProcessWorker() { Id = 10003, ProcessId = 10001, WorkerId = 10003 },
            new ProcessWorker() { Id = 10008, ProcessId = 10004, WorkerId = 10005 },
            new ProcessWorker() { Id = 10009, ProcessId = 10004, WorkerId = 10005 },
            new ProcessWorker() { Id = 10010, ProcessId = 10005, WorkerId = 10006 },
            new ProcessWorker() { Id = 10011, ProcessId = 10005, WorkerId = 10006 },
            new ProcessWorker() { Id = 10012, ProcessId = 10006, WorkerId = 10005 },
            new ProcessWorker() { Id = 10013, ProcessId = 10007, WorkerId = 10006 },
            new ProcessWorker() { Id = 10014, ProcessId = 10008, WorkerId = 10005 },
            new ProcessWorker() { Id = 10015, ProcessId = 10008, WorkerId = 10005 },
            new ProcessWorker() { Id = 10016, ProcessId = 10009, WorkerId = 10006 },

            // ProductionProcess/Plan
            // Assembly
            new ProcessWorker() { Id = 10017, ProcessId = 10100, WorkerId = 10014 },
            new ProcessWorker() { Id = 10018, ProcessId = 10100, WorkerId = 10014 },
            new ProcessWorker() { Id = 10019, ProcessId = 10101, WorkerId = 10014 },
            new ProcessWorker() { Id = 10020, ProcessId = 10101, WorkerId = 10014 },
            // Packing
            new ProcessWorker() { Id = 10021, ProcessId = 10108, WorkerId = 10014 },
            new ProcessWorker() { Id = 10022, ProcessId = 10108, WorkerId = 10014 },
            new ProcessWorker() { Id = 10023, ProcessId = 10109, WorkerId = 10014 },
            new ProcessWorker() { Id = 10024, ProcessId = 10109, WorkerId = 10014 },
            // Sorting
            new ProcessWorker() { Id = 10025, ProcessId = 10110, WorkerId = 10014 },
            new ProcessWorker() { Id = 10026, ProcessId = 10110, WorkerId = 10014 },
            new ProcessWorker() { Id = 10027, ProcessId = 10111, WorkerId = 10014 },
            new ProcessWorker() { Id = 10028, ProcessId = 10111, WorkerId = 10014 },
            // Layout
            new ProcessWorker() { Id = 10029, ProcessId = 10104, WorkerId = 10014 },
            new ProcessWorker() { Id = 10030, ProcessId = 10104, WorkerId = 10014 },
            new ProcessWorker() { Id = 10031, ProcessId = 10105, WorkerId = 10014 },
            new ProcessWorker() { Id = 10032, ProcessId = 10105, WorkerId = 10014 },
            // Cnc
            new ProcessWorker() { Id = 10033, ProcessId = 10102, WorkerId = 10014 },
            new ProcessWorker() { Id = 10034, ProcessId = 10102, WorkerId = 10014 },
            new ProcessWorker() { Id = 10035, ProcessId = 10103, WorkerId = 10014 },
            new ProcessWorker() { Id = 10036, ProcessId = 10103, WorkerId = 10014 },
            // Edgebanding
            new ProcessWorker() { Id = 10037, ProcessId = 10106, WorkerId = 10014 },
            new ProcessWorker() { Id = 10038, ProcessId = 10106, WorkerId = 10014 },
            new ProcessWorker() { Id = 10039, ProcessId = 10107, WorkerId = 10014 },
            new ProcessWorker() { Id = 10040, ProcessId = 10107, WorkerId = 10014 }
        };
    }
}
