using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class ReportSeed : IEntitySeed<Report>
    {
        public Report[] Entities => new[]
        {
            new Report("Report1") { Id = 1 },
            new Report("Report2") { Id = 2 },
            new Report("Report3") { Id = 3 },
            new Report("Report4") { Id = 4 }
        };

        //public Report[] Entities => new Report[] { };
    }
}