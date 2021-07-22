using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class StockReportSeed : IEntitySeed<StockReport>
    {
        public StockReport[] Entities => new[]
        {
            new StockReport(1,1) { Id = 1 },
            new StockReport(1,2) { Id = 2 },
            new StockReport(1,3) { Id = 3 },
            new StockReport(1,4) { Id = 4 }
        };

        //public StockReport[] Entities => new StockReport[] { };
    }
}