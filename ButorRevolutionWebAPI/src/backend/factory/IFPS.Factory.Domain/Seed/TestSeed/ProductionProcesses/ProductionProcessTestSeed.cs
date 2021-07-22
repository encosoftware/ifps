using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class ProductionProcessTestSeed : IEntitySeed<ProductionProcess>
    {
        public ProductionProcess[] Entities => new[]
        {
            // Cuttings
            new ProductionProcess(new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887"),10000) { Id = 10000, StartTime = new DateTime(2019, 8, 5), EndTime = new DateTime(2019, 8, 5).AddMinutes(6)},
            new ProductionProcess(new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887"),10001) { Id = 10001, StartTime = new DateTime(2019, 8, 5), EndTime = new DateTime(2019, 8, 5).AddMinutes(6)},

            // CNC
            new ProductionProcess(new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887"),10002) { Id = 10004 },
            new ProductionProcess(new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887"),10003) { Id = 10005, StartTime = new DateTime(2019, 8, 5), EndTime = new DateTime(2019, 8, 5).AddMinutes(6)},

            // EdgeBanding
            new ProductionProcess(new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887"),10004) { Id = 10006, StartTime = new DateTime(2019, 8, 5), EndTime = new DateTime(2019, 8, 5).AddMinutes(6)},
            new ProductionProcess(new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887"),10005) { Id = 10007, StartTime = new DateTime(2019, 8, 5), EndTime = new DateTime(2019, 8, 5).AddMinutes(6)},

            // Assembly
            new ProductionProcess(new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887"),10006) { Id = 10008, StartTime = new DateTime(2019, 8, 5), EndTime = new DateTime(2019, 8, 5).AddMinutes(6)},
            new ProductionProcess(new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887"),10007) { Id = 10009, StartTime = new DateTime(2019, 8, 5), EndTime = new DateTime(2019, 8, 5).AddMinutes(6)},

            new ProductionProcess(new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887"),10008) { Id = 10010 },

            // These correct datas for only production process/plan testing, please do not change or use for other tests
            // Assembly
            new ProductionProcess(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"), 10009) { Id = 10100 },
            new ProductionProcess(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"), 10010) { Id = 10101 },

            // Cnc
            new ProductionProcess(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"), 10017) { Id = 10102 },
            new ProductionProcess(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"), 10018) { Id = 10103 },

            // Layout
            new ProductionProcess(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"), 10015) { Id = 10104 },
            new ProductionProcess(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"), 10016) { Id = 10105 },

            // Edgebanding
            new ProductionProcess(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"), 10019) { Id = 10106 },
            new ProductionProcess(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"), 10020) { Id = 10107 },

            // Packing
            new ProductionProcess(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"), 10011) { Id = 10108 },
            new ProductionProcess(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"), 10012) { Id = 10109 },

            // Sorting
            new ProductionProcess(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"), 10013) { Id = 10110 },
            new ProductionProcess(new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"), 10014) { Id = 10111 }
        };
    }
}
