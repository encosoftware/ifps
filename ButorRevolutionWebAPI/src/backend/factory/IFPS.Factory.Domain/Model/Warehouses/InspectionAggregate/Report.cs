using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class Report : FullAuditedEntity
    {
        public string Name { get; set; }


        public bool IsClosed { get; set; }
        /// <summary>
        /// Private list of stock reports
        /// </summary>
        private List<StockReport> _stockReports;
        public IEnumerable<StockReport> StockReports => _stockReports.AsReadOnly();

        public Report(string name)
        {
            _stockReports = new List<StockReport>();
            Name = name;
            IsClosed = false;
        }

        /// <summary>
        /// Add new stock report
        /// </summary>
        /// <param name="stockReport"></param>
        public void AddStockReport(StockReport stockReport)
        {
            Ensure.NotNull(stockReport);

            _stockReports.Add(stockReport);
        }
    }
}
