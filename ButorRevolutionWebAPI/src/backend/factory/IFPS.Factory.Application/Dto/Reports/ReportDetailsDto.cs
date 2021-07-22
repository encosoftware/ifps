using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class ReportDetailsDto
    {
        public InspectionListDto Inspection { get; set; }
        public List<StockReportListDto> StockReports { get; set; }

        public ReportDetailsDto(Report report)
        {
            StockReports = report.StockReports
                .Where(sr => !sr.Stock.ValidTo.HasValue)
                .Select(ent => new StockReportListDto(ent.Stock) { Id = ent.Id, MissingAmount = ent.MissingAmount })
                .ToList();
        }
    }
}
