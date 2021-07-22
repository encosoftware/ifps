using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Dbos
{
    public class StockedItemCsvDataDbo
    {
        public string PackageDescription { get; set; }
        public string PackageCode { get; set; }
        public string StorageCellName { get; set; }
        public string StorageCellMetadata { get; set; }
        public int Quantity { get; set; }

        public StockedItemCsvDataDbo(Stock stock)
        {
            PackageDescription = stock.Package.PackageDescription;
            PackageCode = stock.Package.PackageCode;
            StorageCellName = stock.StorageCell.Name;
            StorageCellMetadata = stock.StorageCell.Metadata;
            Quantity = stock.Quantity;
        }
    }
}
