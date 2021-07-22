using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class StockReportListDto
    {
        public int Id { get; set; }
        public string PackageDescription { get; set; }
        public string PackageCode { get; set; }
        public string StorageCellName { get; set; }
        public string StorageCellMetadata { get; set; }
        public int Quantity { get; set; }
        public int? MissingAmount { get; set; }

        public StockReportListDto(Stock stock)
        {
            Id = stock.Id;
            PackageDescription = stock.Package.PackageDescription;
            PackageCode = stock.Package.PackageCode;
            StorageCellName = stock.StorageCell.Name;
            StorageCellMetadata = stock.StorageCell.Metadata;
            Quantity = stock.Quantity;
        }
    }
}
