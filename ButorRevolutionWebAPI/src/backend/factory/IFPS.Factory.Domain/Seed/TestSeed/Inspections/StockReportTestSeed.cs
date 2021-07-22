using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class StockReportTestSeed : IEntitySeed<StockReport>
    {
        public StockReport[] Entities => new[]
        {
            new StockReport(10000,10000) { Id = 10000 },
            new StockReport(10000,10001) { Id = 10001 },
            new StockReport(10000,10002) { Id = 10002 },
            new StockReport(10000,10003) { Id = 10003 }
        };
    }
}