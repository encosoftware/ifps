using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class ReportTestSeed : IEntitySeed<Report>
    {
        public Report[] Entities => new[]
        {
            new Report("Report1") { Id = 10000 },
            new Report("Report2") { Id = 10001 },
            new Report("Report3") { Id = 10002 },
            new Report("Report4") { Id = 10003 }
        };
    }
}