using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class InspectionSeed : IEntitySeed<Inspection>
    {
        public Inspection[] Entities => new[]
        {
            new Inspection(new DateTime(2019,7,19),1) { Id = 1, ReportId = 1 },
            new Inspection(new DateTime(2019,8,19),1) { Id = 2, ReportId = 2 },
            new Inspection(new DateTime(2019,9,19),2) { Id = 3, ReportId = 3 },
            new Inspection(new DateTime(2019,10,19),2) { Id = 4, ReportId = 4 }
        };
        
        //public Inspection[] Entities => new Inspection[] { };
    }
}