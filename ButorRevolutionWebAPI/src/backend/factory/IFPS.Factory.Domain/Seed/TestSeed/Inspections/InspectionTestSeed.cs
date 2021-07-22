using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class InspectionTestSeed : IEntitySeed<Inspection>
    {
        public Inspection[] Entities => new[]
        {
            new Inspection(new DateTime(2019,10,19),10000) { Id = 10000, ReportId = 10000 },
            new Inspection(new DateTime(2019,10,19),10001) { Id = 10001, ReportId = 10001 },
            new Inspection(new DateTime(2019,10,19),10002) { Id = 10002, ReportId = 10002 },
            new Inspection(new DateTime(2019,10,19),10003) { Id = 10003, ReportId = 10003 }
        };
    }
}