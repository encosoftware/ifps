using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Factory.Domain.Model
{
    public class StockReport : FullAuditedEntity
    {
        public virtual Stock Stock { get; set; }
        public int StockId { get; set; }

        public virtual Report Report { get; set; }
        public int ReportId { get; set; }

        public int? MissingAmount { get; set; }

        public StockReport(int stockId, int reportId)
        {
            StockId = stockId;
            ReportId = reportId;
        }
    }
}
